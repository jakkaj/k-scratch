using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KScratch.Contract.Services;
using KScratch.Portable.Glue;
using Autofac;

namespace KScratchLog
{
    public class Program
    {
        private static CoreGlue _glue;
        private static IKuduLogService _logService;
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please pass in path to publish settings xml file");
                return;
            }

            _glue = new CoreGlue();
            _glue.Init();

            var isStreaming = _streamLog(args[0]);

            if (!isStreaming)
            {
                Console.WriteLine("There was a problem with that file");
                return;
            }

            Console.ReadKey();
        }

        static bool _streamLog(string settings)
        {
            var pubSettingsService = _glue.Container.Resolve<IPublishSettingsService>();
            var profile = pubSettingsService.LoadPublishProfile(settings);

            if (profile == null)
            {
                return false;
            }

            var kuduProfile = pubSettingsService.GetSettingsByPublishMethod("MSDeploy");

            if (kuduProfile == null)
            {
                return false;
            }

            _logService = _glue.Container.Resolve<IKuduLogService>();

            
            _logService.StartLog(kuduProfile, Console.WriteLine);

            return true;
        }
    }
}
