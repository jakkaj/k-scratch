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

namespace ks.model.Services
{
    public class KuduFileService : IKuduFileService
    {
        private readonly IPublishSettingsService _publishSettingsService;
        private readonly ILocalLogService _localLogService;

        private KuduSiteSettings _siteSettings;

        private FileSystemWatcher _watcher;

        public KuduFileService(IPublishSettingsService publishSettingsService, 
            ILocalLogService localLogService)
        {
            _publishSettingsService = publishSettingsService;
            _localLogService = localLogService;
        }

        public void Monitor()
        {

            var baseDir = Directory.GetCurrentDirectory();

            _watcher = new FileSystemWatcher(baseDir);
            _watcher.IncludeSubdirectories = true;


            _watcher.Changed += Watcher_Changed;
            _watcher.Created += Watcher_Created;

            _watcher.EnableRaisingEvents = true;

            _localLogService.Log($"Monitoring {baseDir} for changes");
        }

        async void _disable()
        {
            _watcher.EnableRaisingEvents = false;
            await Task.Delay(1000);
            _watcher.EnableRaisingEvents = true;
        }

        private async void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            _disable();
            var f = e.FullPath;
            _localLogService.Log($"Detected file creation {f}");
            await SendFile(f);

        }

        private async void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            _disable();
               var f = e.FullPath;
            _localLogService.Log($"Detected file change {f}");
            await SendFile(f);
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
                _localLogService.Log($"Error. Could not read {fullPath}. Probably locked or something like that.");
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

        public async Task PullFiles()
        {
            _siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                var requestUri = $"https://{_siteSettings.ApiUrl}/api/zip/site/wwwroot/";

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

                var result = await response.Content.ReadAsByteArrayAsync();

                var saveFile = Path.GetTempFileName();

                File.WriteAllBytes(saveFile, result);

                ZipFile.ExtractToDirectory(saveFile, Directory.GetCurrentDirectory());

                File.Delete(saveFile);
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
