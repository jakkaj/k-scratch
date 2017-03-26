using ks.model.Contract.Repos;
using ks.model.Entity;
using ks.model.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ks.model.Services
{
    public class TestService : ITestService
    {
        readonly IFileRepo _fileRepo;
        readonly IFunctionSettingsService _functionSettings;

        public TestService(IFileRepo fileRepo, IFunctionSettingsService functionSettings)
        {
            this._functionSettings = functionSettings;
            this._fileRepo = fileRepo;
        }

        public async Task<bool> RunTest(int testNumber)
        {
            var settings = await _functionSettings.GetFunctionSettings();

            if(settings == null)
            {
                return false;
            }

            if(settings.Count < testNumber)
            {
                return false;
            }

            var setting = settings[testNumber - 1];

            //find the direction

            var binding = setting.config.bindings.Where(
                _ => _.direction == KsConstants.Functions.In && 
                _.type.EndsWith(KsConstants.Functions.Trigger, StringComparison.Ordinal))
                .FirstOrDefault();

            if(binding == null)
            {
                //erm? 
                return false;
            }

            if(binding.type == KsConstants.Functions.HttpTrigger)
            {
                //httptrigger, so this one needs to be called via direct http
            }
            else
            {
                //other trigger, so call this one via admin api
            }

            return true;
            
        }
    }
}
