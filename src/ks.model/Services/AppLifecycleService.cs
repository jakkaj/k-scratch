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

        public (int, bool) Init(string[] args)
        {
            //check for publish settings file. 

            _logService.Log("");
            _logService.Log("Welcome to k-scratch. v0.1.3");
            _logService.Log("Documentation and code @ https://github.com/jakkaj/k-scratch");

            //just a couple of lines to space things out
            
            _logService.Log("");
            var processor = _commandProcessorService.Process(args);

            return processor;
        }
    }
}
