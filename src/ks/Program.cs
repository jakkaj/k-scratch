using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ks.Glue;
using ks.Model.Contract.Services;
using ks.Model.Glue;
using ks.Model.Services;

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

            if (initResult != 0)
            {
                return initResult;
            }

            Console.ReadKey();

            return initResult;
        }
    }
}
