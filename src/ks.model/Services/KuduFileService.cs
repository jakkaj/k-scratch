using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ks.model.Contract;
using ks.model.Contract.Services;
using ks.model.Entity.Enum;
using ks.model.Entity.Kudu;
using ks.model.Helpers;
using System.Collections.Generic;

namespace ks.model.Services
{
    public class KuduFileService : IKuduFileService
    {
        private readonly IPublishSettingsService _publishSettingsService;
        private readonly ILocalLogService _localLogService;

        private KuduSiteSettings _siteSettings;

        private FileSystemWatcher _watcher;

        System.Threading.Timer _timer;

        private List<string> _filesChangedList = new List<string>();

        static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        readonly IFileWatcherService _fileWatcherService;

        public KuduFileService(IPublishSettingsService publishSettingsService,
            ILocalLogService localLogService, IFileWatcherService fileWatcherService)
        {
            _fileWatcherService = fileWatcherService;
            _publishSettingsService = publishSettingsService;
            _localLogService = localLogService;
        }

        public void Monitor()
        {

            var baseDir = Directory.GetCurrentDirectory();

            _fileWatcherService.Watch(baseDir, async (f) =>
            {
                await SendFile(f);
            });
        }       

        public async Task SendFile(string fullPath)
        {
            if (!File.Exists(fullPath))
            {
                return;
            }

            byte[] data = null;

            try
            {
                data = File.ReadAllBytes(fullPath);
            }
            catch (Exception ex)
            {
                _localLogService.Log($"Error. Could not read {fullPath}. Probably locked or something like that. {ex.ToString()}");
                return;
            }


            var baseDir = Directory.GetCurrentDirectory();

            var offsetPath = fullPath.Replace(baseDir, "");

            _siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);

            var offsetForUrl = offsetPath.Replace('\\', '/');

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                var requestUri = $"https://{_siteSettings.ApiUrl}/api/vfs/site/wwwroot{offsetForUrl}";

                var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                   HttpHelpers.GetAuthenticationString(_siteSettings));

                var content = new ByteArrayContent(data);
                request.Content = content;
                request.Headers.Add("If-Match", "*");
                var result = await httpClient.SendAsync(request);

                if (result.IsSuccessStatusCode)
                {
                    _localLogService.Log($"Sent {requestUri}");
                }
                else
                {
                    _localLogService.Log($"Failed {requestUri}, {result.ToString()}");
                }
            }
        }

        public async Task<bool> UploadFiles(string subPath = null)
        {
            _siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);

            var dir = Directory.GetCurrentDirectory();
            if (!string.IsNullOrEmpty(subPath))
            {
                dir += $"\\{subPath}";
            }

            var tmpFile = Path.GetTempFileName();
            if (File.Exists(tmpFile))
            {
                File.Delete(tmpFile);
            }
            ZipFile.CreateFromDirectory(dir, tmpFile);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMinutes(10);

                var requestUri = $"https://{_siteSettings.ApiUrl}/api/zip/site/wwwroot/";

                if (!string.IsNullOrEmpty(subPath))
                {
                    subPath = subPath.Trim('/').Trim('\\');
                    requestUri += $"{subPath}/";
                }

                var request = new HttpRequestMessage(HttpMethod.Put, requestUri);

                try
                {
                    using (var stream = File.OpenRead(tmpFile))
                    {
                        request.Content = new StreamContent(File.OpenRead(tmpFile));

                        request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                            HttpHelpers.GetAuthenticationString(_siteSettings));

                        var response = await httpClient.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            _localLogService.Log($"Error: Could not upload files files to {requestUri} - {response.ReasonPhrase}");
                            return false;
                        }

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _localLogService.Log($"Problem uploadin files: {ex.Message}");
                }
                finally
                {
                    File.Delete(tmpFile);
                }

                return false;
            }
        }

        public async Task<bool> GetFiles(string subPath = null)
        {
            _siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                var requestUri = $"https://{_siteSettings.ApiUrl}/api/zip/site/wwwroot/";

                if (!string.IsNullOrEmpty(subPath))
                {
                    subPath = subPath.Trim('/').Trim('\\');
                    requestUri += $"{subPath}/";
                }

                _localLogService.Log($"Grabbing from {requestUri}");

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                    HttpHelpers.GetAuthenticationString(_siteSettings));

                var response = await httpClient.SendAsync(
                   request, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    _localLogService.Log($"Error: Count not get files from {requestUri}");
                    return false;
                }

                var result = await response.Content.ReadAsByteArrayAsync();

                var saveFile = Path.GetTempFileName();

                File.WriteAllBytes(saveFile, result);

                try
                {
                    var dir = Directory.GetCurrentDirectory();
                    if (!string.IsNullOrEmpty(subPath))
                    {
                        dir += $"\\{subPath}";
                    }

                    ZipFile.ExtractToDirectory(saveFile, dir);

                    foreach (var f in Directory.GetFiles(dir, "*", SearchOption.AllDirectories))
                    {
                        if (_fileWatcherService.Validate(f))
                        {
                            _localLogService.Log($" - {f.Replace(dir, "")}");
                        }
                    }
                }
                catch (System.IO.IOException ex)
                {
                    _localLogService.LogWarning(
                        "k-scratch will not overwrite existing files. Please 'get' in to an empty directory.");
                    return false;
                }
                finally
                {
                    File.Delete(saveFile);
                }

                return true;
            }
        }

        public async Task ListFiles()
        {
            _siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);


            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                var requestUri = $"https://{_siteSettings.ApiUrl}/api/vfs/";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                    HttpHelpers.GetAuthenticationString(_siteSettings));

                var response = await httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    _localLogService.Log($"Error: Count not get files from {requestUri}");
                    return;
                }

                var result = await response.Content.ReadAsStringAsync();

                var r = result;
            }
        }
    }
}
