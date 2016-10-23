using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ks
{
    public static class Commands
    {
        public static Command LogCommand = new Command
        {
            CommandName =  "log",
            CommandHint = "Stream the log to the command window"
        };

    }

    public class Command
    {
        public string CommandName { get; set; }
        public string CommandHint { get; set; }
    }
}
