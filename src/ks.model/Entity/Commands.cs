﻿namespace ks.model.Entity
{
    public static class Commands
    {
        public static Command LogCommand = new Command
        {
            CommandName =  "log",
            CommandHint = "Stream the log to the command window"
        };

        public static Command MonitorCommand = new Command
        {
            CommandName = "monitor",
            CommandHint = "Monitor for changes and upload to Kudu"
        };

    }

    public class Command
    {
        public string CommandName { get; set; }
        public string CommandHint { get; set; }
    }
}
