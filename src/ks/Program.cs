using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ks.Glue;
using ks.model.Contract.Services;
using ks.model.Glue;


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

            Console.ReadKey();

            if (initResult != 0)
            {
                return initResult;
            }

            

            return initResult;
        }
    }
}
