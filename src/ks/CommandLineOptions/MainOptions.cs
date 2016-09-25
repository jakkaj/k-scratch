using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

namespace ks.CommandLineOptions
{
    public class MainOptions
    {
        [Option('c', "clone",
       HelpText = "Set up local file from Kudu. This must be done first. ")]
        public bool Clone { get; set; }

        [Option('g', "get",
        HelpText = "Get files from Kudu. Leave blank to get all")]
        public IEnumerable<string> GetFiles { get; set; }

        [Option('p', "put",
       HelpText = "Send files to Kudu. Leave blank to send all")]
        public IEnumerable<string> PutFiles { get; set; }




    }
}
