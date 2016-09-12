using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KScratch.Contract.Services;
using KScratch.Entity.AzureEntities;
using KScratch.Entity.Kudu;
using KScratch.Portable.Helpers;

namespace KScratch.Portable.Services
{
    public class KuduLogService : IKuduLogService
    {
        private Stream _currentStream;

        public void StopLog()
        {
            if (_currentStream != null)
            {
                _currentStream.Dispose();
                _currentStream = null;
            }
        }

        public async Task<bool> StartLog(KuduSiteSettings settings, 
            Action<string> callback)
        {
            StopLog();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                var requestUri = $"https://{settings.ApiUrl}/logstream";

                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                    HttpHelpers.GetAuthenticationString(settings));

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
                        while (!reader.EndOfStream && _currentStream != null)
                        {
                            //We are ready to read the stream
                            var currentLine = reader.ReadLine();
                            Debug.WriteLine(currentLine);
                            callback(currentLine);
                            await Task.Yield();
                        }
                    }
                });

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                return true;
            }

        }
    }
}
