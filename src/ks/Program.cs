using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ks.CommandLineOptions;
using CommandLine;
using ks.Glue;
using KScratch.Portable.Glue;

namespace ks
{
    public class Program
    {
        static CoreGlue _glue;
        public static int Main(string[] args)
        {
            _glue = new CoreGlue();
            _glue.Init(new KSModule());

            var process = _glue.Container.Resolve<Process>();

            var result = CommandLine.Parser.Default.ParseArguments<MainOptions>(args);

            var exitCode = result
              .MapResult(
                options => process.ProcessOptions(options),
                errors => 1);
                    return exitCode;
                }
    }
}
