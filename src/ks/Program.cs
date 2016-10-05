using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
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

            CommandProcessor process = _glue.Container.Resolve<CommandProcessor>();

            var result = process.Process(args);

            Console.WriteLine(result);

            Console.ReadLine();

            return result;
        }
    }
}
