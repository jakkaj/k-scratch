using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ks.Glue;
using ks.model.Contract.Services;
using ks.model.Glue;
using ks.model.Services;

namespace ks
{
    public class Program
    {
        static CoreGlue _glue;

        public static int Main(string[] args)
        {
            _glue = new CoreGlue();
            _glue.Init(new KSModule());


            var initService = _glue.Container.Resolve<IAppLifecycleService>();

            var initResult = initService.Init(args);

            Console.WriteLine("");
            Console.WriteLine("");

            if (!initResult.Item2) return initResult.Item1;

            var testService = _glue.Container.Resolve<ITestService>();

            while (true)
            {
                var key = Console.ReadKey();

                if(key.Key == ConsoleKey.Escape)
                {
                    return initResult.Item1;
                }

                var i = (int)key.Key;

                //49 is 1. We want to start from 0 @ 49
                if(i >= 49 && i <= 57)
                {
                    var numb = i - 48;

                    if (numb >= 0)
                    {
                        testService.RunTest(numb);
                    }
                }                

                System.Threading.Thread.Sleep(250);
            }
            

            return initResult.Item1;
        }
    }
}
