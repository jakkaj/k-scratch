using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ks.model.Contract.Services;
using ks.model.Glue;

namespace ks.integrationtests
{

    public class TestBase
    {
        static CoreGlue _glue;

        public TestBase()
        {
            _glue = new CoreGlue();
            _glue.Init();
        }

        public async Task Init()
        {
            Directory.SetCurrentDirectory(@"C:\Temp\streamrip\Timelapser");
            var settings = Resolve<IPublishSettingsService>();
            var pubSettings = settings.AutoLoadPublishProfile();

            if (pubSettings == null)
            {
                throw new Exception("Could not find publish settings file.");
              
            }
        }

        public T Resolve<T>()
        {
            return _glue.Container.Resolve<T>();
        }
    }
}
