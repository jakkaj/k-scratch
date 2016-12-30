using System;
using System.CommandLine;
using System.Threading.Tasks;
using ks.model.Contract.Services;
using ks.model.Entity;

namespace ks.model.Services
{
    public class CommandProcessorService : ICommandProcessorService
    {
        private readonly ILocalLogService _logService;
        private readonly IKuduLogService _kuduLogService;

        public CommandProcessorService(ILocalLogService logService, IKuduLogService kuduLogService)
        {
            _logService = logService;
            _kuduLogService = kuduLogService;
        }
        public int Process(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                args = new[] { "-h" };
            }

            _logService.Log("Welcome to k-scratch. Someversion number.");

            var command = string.Empty;

            ArgumentSyntax.Parse(args, syntax =>
            {
                //syntax.DefineOption("n|name", ref addressee, "The addressee to greet");

                syntax.DefineCommand(Commands.LogCommand.CommandName, ref command, Commands.LogCommand.CommandHint);
                //syntax.DefineOption("p|prune", ref prune, "Prune branches");

                //syntax.DefineCommand("commit", ref command, "Committing changes");
                //syntax.DefineOption("m|message", ref message, "The message to use");
                //syntax.DefineOption("amend", ref amend, "Amend existing commit");
            });

            if (!_checkCommands(command))
            {
                Console.WriteLine("I don't know that! Try -h to see what I do know.");
            }



            return 0;
        }

        bool _checkCommands(string command)
        {
            if (Commands.LogCommand.CommandName == command)
            {
                Task.Run(() =>
                {
                    _kuduLogService.StartLog();
                });
                return true;
            }

            return false;
        }
    }
}
