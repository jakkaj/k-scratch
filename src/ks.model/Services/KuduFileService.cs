using System;
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

        public KuduFileService(IPublishSettingsService publishSettingsService, 
            ILocalLogService localLogService)
        {
            _publishSettingsService = publishSettingsService;
            _localLogService = localLogService;
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
                    return;
                }

                var result = await response.Content.ReadAsStringAsync();

                var r = result;
            }
        }
    }
}
