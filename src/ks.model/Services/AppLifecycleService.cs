using ks.model.Contract.Services;

namespace ks.model.Services
{
    public class AppLifecycleService : IAppLifecycleService
    {
        private readonly ILocalLogService _logService;
        private readonly IPublishSettingsService _publishSettingsService;
        private readonly ICommandProcessorService _commandProcessorService;

        public AppLifecycleService(ILocalLogService logService, 
            IPublishSettingsService publishSettingsService,
            ICommandProcessorService commandProcessorService)
        {
            _logService = logService;
            _publishSettingsService = publishSettingsService;
            _commandProcessorService = commandProcessorService;
        }

        public int Init(string[] args)
        {
            //check for publish settings file. 

            var pubSettings = _publishSettingsService.AutoLoadPublishProfile();

            if (pubSettings == null)
            {
                _logService.Log("Could not find publish settings file.");
                return 2;
            }

            var processor = _commandProcessorService.Process(args);

            return processor;
        }
    }
}
