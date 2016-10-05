using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;

namespace ks
{
    public class CommandProcessor
    {
        public int Process(string[] args)
        {
            var addressee = "world";

            if (args == null || args.Length == 0)
            {
                args = new[] {"-h"};
            }

            var command = string.Empty;
            var prune = false;
            var message = string.Empty;
            var amend = false;

            ArgumentSyntax.Parse(args, syntax =>
            {
                //syntax.DefineOption("n|name", ref addressee, "The addressee to greet");

                syntax.DefineCommand("pull", ref command, "Pull from another repo");
                syntax.DefineOption("p|prune", ref prune, "Prune branches");

                syntax.DefineCommand("commit", ref command, "Committing changes");
                syntax.DefineOption("m|message", ref message, "The message to use");
                syntax.DefineOption("amend", ref amend, "Amend existing commit");
            });



            Console.WriteLine($"Command {command}, Prune {prune}, Message {message}, Amend {amend}");
            return 0;
        }
    }
}
