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

            //var process = _glue.Container.Resolve<Process>();

            var addressee = "world";

            ArgumentSyntax.Parse(args, syntax =>
            {
                syntax.DefineOption("n|name", ref addressee, "The addressee to greet");
            });

            Console.WriteLine("Hello {0}!", addressee);



            return 0;
        }
    }
}
