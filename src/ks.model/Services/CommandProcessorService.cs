﻿using System;
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
        public (int, bool) Process(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                args = new[] { "-h" };
            }

            var monitor = false;
            var log = false;
            var get = false;
            var path = string.Empty;
            var folder = string.Empty;

            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("l|log", ref log, "Output the Kudulog stream to the console");
                syntax.DefineOption("m|monitor", ref monitor, "Monitor the path for changes and send them up");
                syntax.DefineOption("p|path", ref path, "The base path of your function (blank for current path)");
                syntax.DefineOption("g|get", ref get, "Download the Function app ready for editing locally");
                syntax.DefineOption("f|folder", ref folder, "Sub folder to get. If omitted it will get everything under wwwroot from Kudu");
            });

            if (!string.IsNullOrEmpty(path))
            {
                _logService.LogInfo($"Base path: {path}");
                if (!Directory.Exists(path))
                {
                    _logService.LogError("Directory does not exist");
                    return (1, false);
                }
                Directory.SetCurrentDirectory(path);
            }

            var pubSettings = _publishSettingsService.AutoLoadPublishProfile();

            if (pubSettings == null)
            {
                _logService.LogError("Could not find publish settings file.");
                _logService.LogInfo("You can download a publish settings file from your Azure App Service settings.");
                _logService.LogInfo("Sample video: https://cloud.githubusercontent.com/assets/5225782/23344608/ac7c44d4-fcd3-11e6-90f2-0291a31f1522.gif");
                return (0, false);
            }

            if (get)
            {
                var filesResult = _fileService.GetFiles(folder).Result;

                if (!filesResult)
                {
                    return (1, false);
                }
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
                return (0, true);
            }

            return (0, false);
        }
    }
}
