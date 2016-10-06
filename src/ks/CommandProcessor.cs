using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using KScratch.Contract.Services;
namespace ks
{
    public class CommandProcessor
    {
        private readonly ILocalLogService _logService;

        public CommandProcessor(ILocalLogService logService)
        {
            _logService = logService;
        }
        public int Process(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                args = new[] {"-h"};
            }

            _logService.Log("Welcome to k-scratch. Someversion number.");

            var command = string.Empty;
            var prune = false;
            var message = string.Empty;
            var amend = false;

            ArgumentSyntax.Parse(args, syntax =>
            {
                //syntax.DefineOption("n|name", ref addressee, "The addressee to greet");

                syntax.DefineCommand("log", ref command, "Stream the log to the command window");
                //syntax.DefineOption("p|prune", ref prune, "Prune branches");

                //syntax.DefineCommand("commit", ref command, "Committing changes");
                //syntax.DefineOption("m|message", ref message, "The message to use");
                //syntax.DefineOption("amend", ref amend, "Amend existing commit");
            });



            Console.WriteLine($"Command {command}, Prune {prune}, Message {message}, Amend {amend}");
            return 0;
        }
    }
}
