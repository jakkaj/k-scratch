using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ks.model.Contract;
using ks.model.Contract.Services;
using ks.model.Entity;

namespace ks.model.Services
{
    public class CommandProcessorService : ICommandProcessorService
    {
        private readonly ILocalLogService _logService;
        private readonly IKuduLogService _kuduLogService;
        private readonly IKuduFileService _fileService;
        private readonly IPublishSettingsService _publishSettingsService;

        public CommandProcessorService(ILocalLogService logService,
            IKuduLogService kuduLogService, IKuduFileService fileService, IPublishSettingsService publishSettingsService)
        {
            _logService = logService;
            _kuduLogService = kuduLogService;
            _fileService = fileService;
            _publishSettingsService = publishSettingsService;
        }
        public int Process(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                args = new[] { "-h" };
            }

           

            bool monitor = false;
            bool log = false;
            bool get = false;
            var path = string.Empty;

            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("l|log", ref log, "Output the Kudulog stream to the console");
                syntax.DefineOption("m|monitor", ref monitor, "Monitor the path for changes and send them up");
                syntax.DefineOption("p|path", ref path, "The base path of your function (blank for current path)");
                syntax.DefineOption("g|get", ref get, "Download the Function app ready for editing locally");


            });

            if (!string.IsNullOrEmpty(path))
            {
                _logService.Log($"Setting base path to {path}");
                Directory.SetCurrentDirectory(path);
                _publishSettingsService.AutoLoadPublishProfile();


            }

            if (get)
            {
                _fileService.GetFiles().Wait();
            }


            if (log)
            {
                Task.Run(() =>
                {
                    _kuduLogService.StartLog();
                });
            }

            if (monitor)
            {
                Task.Run(() =>
                {
                    _fileService.Monitor();
                });
            }


            return 0;
        }
    }
}
