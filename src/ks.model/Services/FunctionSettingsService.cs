using ExtensionGoo.Standard.Extensions;
using ks.model.Contract.Services;
using ks.model.Entity.Enum;
using ks.model.Entity.Kudu;
using ks.model.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ks.model.Services
{
    public class FunctionSettingsService : IFunctionSettingsService
    {
        readonly IPublishSettingsService _publishSettingsService;
        readonly ILocalLogService _logService;

        public FunctionSettingsService(IPublishSettingsService publishSettingsService,
            ILocalLogService logService)
        {
            this._logService = logService;
            this._publishSettingsService = publishSettingsService;
        }
        public async Task<List<FunctionSettings>> GetFunctionSettings()
        {            
            var siteSettings = _publishSettingsService.GetSettingsByPublishMethod(PublishMethods.MSDeploy);
            var requestUri = $"https://{siteSettings.ApiUrl}/api/functions";

            var result = await requestUri.GetAndParse<List<FunctionSettings>>(HttpHelpers.GetAuthHeadersForGoo(siteSettings));

            return result;
        }
    }
}
