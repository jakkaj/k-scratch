using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ks.model.Contract.Services;
using ks.model.Entity.Enum;
using ks.model.Entity.Kudu;
using ks.model.Helpers;

namespace ks.model.Services
{
    public class KuduLogService : IKuduLogService
    {
        private readonly IPublishSettingsService _publishSettingsService;
        private readonly ILocalLogService _localLogService;
        private Stream _currentStream;
        private KuduSiteSettings _siteSettings;

        public KuduLogService(IPublishSettingsService publishSettingsService, ILocalLogService localLogService)
        {
            _publishSettingsService = publishSettingsService;
            _localLogService = localLogService;
          
        }
        public void StopLog()
        {
            if (_currentStream != null)
            {
                _currentStream.Dispose();
                _currentStream = null;
            }
        }

        public async Task<bool> StartLog()
        {
            _localLogService.LogInfo("Starting logstream");
            _siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);
            StopLog();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                var requestUri = $"https://{_siteSettings.ApiUrl}/logstream";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                    HttpHelpers.GetAuthenticationString(_siteSettings));

                var response = await httpClient.SendAsync(
                    request, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                _currentStream = await response.Content.ReadAsStreamAsync();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
                {
                    using (var reader = new StreamReader(_currentStream))
                    {
                        var previous = new List<string>();
                        while (!reader.EndOfStream && _currentStream != null)
                        {
                            //We are ready to read the stream
                            
                            var currentLine = await reader.ReadLineAsync();

                            //it doubles up for some reason :/
                            if (previous.Contains(currentLine))
                            {
                                continue;
                            }

                            previous.Add(currentLine);

                            _localLogService.Log($" > {currentLine}");
                        }
                    }
                });

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

        }
    }
}
