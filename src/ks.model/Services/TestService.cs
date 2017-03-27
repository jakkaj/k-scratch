using ExtensionGoo.Standard.Extensions;
using ks.model.Contract.Repos;
using ks.model.Contract.Services;
using ks.model.Entity;
using ks.model.Entity.Enum;
using ks.model.Entity.Kudu;
using ks.model.Entity.Kudu.TestData;

using ks.model.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ks.model.Services
{
    public class TestService : ITestService
    {
        readonly IFileRepo _fileRepo;
        readonly IFunctionSettingsService _functionSettings;
        readonly IPublishSettingsService _pubSettingsService;
        readonly ILocalLogService _localLogService;
        readonly IParamService _paramService;

        private List<FunctionSettings> _settings;

        public TestService(IFileRepo fileRepo, 
            IFunctionSettingsService functionSettings,
            IPublishSettingsService pubSettingsService, 
            ILocalLogService localLogService,
            IParamService paramService)
        {
            this._paramService = paramService;
            this._localLogService = localLogService;
            this._pubSettingsService = pubSettingsService;
            this._functionSettings = functionSettings;
            this._fileRepo = fileRepo;
        }

        async Task<string> _getKey(KuduSiteSettings siteSettings, FunctionSettings funcSettings)
        {
            var requestUri = $"{siteSettings.LiveUrl.Replace("http", "https")}/admin/functions/{funcSettings.name}/keys";

            var funcKey = _paramService.Get("key");

            var headers = new Dictionary<string, string>();

            headers.Add("x-functions-key", funcKey);

            var result = await requestUri.GetAndParse<FunctionKey>(headers);

            if(result == null)
            {
                return null;
            }

            var k = result.keys.FirstOrDefault(_ => _.name == KsConstants.Functions.Default);

            return k?.value;            
        }

        public async Task<bool> GetFunctionData()
        {
            _settings = await _functionSettings.GetFunctionSettings();

            _localLogService.Log("Refreshing functions");

            if (_settings == null)
            {
                _localLogService.LogError("Could not get function settings! Check the function key passed in with the 'k' parameter.");
                return false;
            }

            _localLogService.LogInfo("    Functions in this site \r\n          press (number) to run test data on live Azure Function. Press (r) to refresh function data from server.");

            var count = 1;

            foreach (var s in _settings)
            {
                var binding = s.config.bindings
               .FirstOrDefault(_ => _.direction == KsConstants.Functions.In &&
               _.type.EndsWith(KsConstants.Functions.Trigger, StringComparison.Ordinal));

                _localLogService.LogInfo($"    ({count}) {s.name} [{binding?.type}]");
                count++;
            }

            return true;
        }

        public async Task<bool> RunTest(int testNumber)
        {
            var funcKey = _paramService.Get("key");

            if (string.IsNullOrWhiteSpace(funcKey))
            {
                _localLogService.LogWarning("Cannot perform test calls without a function key - pass in with the k option. More info on how to get key at project site");
                return false;
            }

            if(_settings == null)
            {
                return false;
            }

            if(_settings.Count < testNumber)
            {
                return false;
            }

            var setting = _settings[testNumber - 1];

            //find the direction

            var binding = setting.config.bindings
                .FirstOrDefault(_ => _.direction == KsConstants.Functions.In && 
                _.type.EndsWith(KsConstants.Functions.Trigger, StringComparison.Ordinal));

            

            if(binding == null)
            {
                _localLogService.LogError($"Could not find an input binding for function {setting.name}");
                //erm? 
                return false;
            }

            _localLogService.LogInfo($" > Running function {setting.name} ({binding.type}) with data {setting.test_data}");

            var siteSettings = _pubSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);

            if (binding.type == KsConstants.Functions.HttpTrigger)
            {
                //httptrigger, so this one needs to be called via direct http
                var confRaw = setting.test_data;
                var conf = JsonConvert.DeserializeObject<TestDataConfig>(confRaw);

                var urlBase = $"{siteSettings.LiveUrl}/api/{setting.name}?";

                foreach(var qs in conf?.queryStringParams)
                {
                    urlBase += $"{WebUtility.UrlEncode(qs.name)}={WebUtility.UrlEncode(qs.value)}&";
                }

                urlBase += $"code={await _getKey(siteSettings, setting)}";

                return await _doRequest(urlBase, conf.method, conf.headers, conf.body, siteSettings);             

            }
            else
            {
                //other trigger, so call this one via admin api
                var urlBase = $"{siteSettings.LiveUrl}/admin/functions/{setting.name}";
                var headers = new Dictionary<string, string>();
                headers.Add("x-functions-key", funcKey);

                var post = $"{{\"input\":\"{setting.test_data}\"}}";

                var result = await urlBase.Post(post, headers);
            }

            return true;
            
        }

        async Task<bool> _doRequest(string url, string stringMethod,
            List<NameValuePair> headers, 
            string body, KuduSiteSettings settings)
        {

            _localLogService.LogInfo($" > Calling: {url}");

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(10000);

                var requestUri = url;

                HttpMethod method = default(HttpMethod);

                switch (stringMethod)
                {
                    case "get":
                        method = HttpMethod.Get;
                        break;
                    case "put":
                        method = HttpMethod.Put;
                        break;
                    case "delete":
                        method = HttpMethod.Delete;
                        break;
                    case "post":
                        method = HttpMethod.Post;
                        break;
                    case "head":
                        method = HttpMethod.Head;
                        break;
                    case "options":
                        method = HttpMethod.Options;
                        break;
                    case null:
                        method = HttpMethod.Get;
                        break;
                    default:
                        method = HttpMethod.Get;
                        break;
                        //note that patch is not supported by Httpmethod!
                }

                var request = new HttpRequestMessage(method, requestUri);

                request.Headers.Authorization = new AuthenticationHeaderValue("Basic",
                   HttpHelpers.GetAuthenticationString(settings));

                if(headers != null)
                {
                    foreach(var h in headers)
                    {
                        request.Headers.Add(h.name, h.value);
                    }
                }

                if(body != null)
                {
                    var content = new StringContent(body);
                    request.Content = content;
                }
                
                var result = await httpClient.SendAsync(request);
                
                if (!result.IsSuccessStatusCode)
                {
                    _localLogService.LogError($">>> {await result.Content.ReadAsStringAsync()}");
                    _localLogService.LogError($">>> Code: {result.StatusCode}");
                    _localLogService.LogError($">>> Reason: {result.ReasonPhrase}");
                    return false;
                }
                else
                {
                    _localLogService.Log($">>> {await result.Content.ReadAsStringAsync()}");
                    _localLogService.Log($">>> Code: {result.StatusCode}");
                    return true;
                }
            }
        }
    }
}
