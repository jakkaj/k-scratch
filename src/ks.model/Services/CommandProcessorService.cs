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

            _logService.Log("Welcome to k-scratch. Someversion number.");

            bool monitor = false;
            bool log = false;
            var path = string.Empty;
           
            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("l|log", ref log, "Output the Kudu log stream to the console");
                syntax.DefineOption("m|monitor", ref monitor, "Monitor the path for changes and send them up");

                //syntax.DefineCommand(Commands.MonitorCommand.CommandName, ref monitor, Commands.MonitorCommand.CommandHint);
                syntax.DefineOption("p|path", ref path, "The base path of your function (blank for current path)");
              //  syntax.DefineCommand(Commands.LogCommand.CommandName, ref log, Commands.LogCommand.CommandHint);

            

                //
                //syntax.DefineOption("p|prune", ref prune, "Prune branches");

                //syntax.DefineCommand("commit", ref command, "Committing changes");
                //syntax.DefineOption("m|message", ref message, "The message to use");
                //syntax.DefineOption("amend", ref amend, "Amend existing commit");
            });

            if (!string.IsNullOrEmpty(path))
            {
                _logService.Log($"Setting base path to {path}");
                Directory.SetCurrentDirectory(path);
                _publishSettingsService.AutoLoadPublishProfile();


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

        bool _checkCommands(params string[] command)
        {


            if (command.Contains(Commands.LogCommand.CommandName))
            {
                Task.Run(() =>
                {
                    _kuduLogService.StartLog();
                });
                return true;
            }

            if (command.Contains(Commands.MonitorCommand.CommandName))
            {
                Task.Run(() =>
                {
                    _fileService.Monitor();
                });
                return true;
            }

            return false;
        }
    }
}
