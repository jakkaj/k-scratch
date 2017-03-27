using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ks.Glue;
using ks.model.Contract.Services;
using ks.model.Glue;

namespace ks.unittests
{

    public class TestBase
    {
        static CoreGlue _glue;

        public TestBase()
        {
            _glue = new CoreGlue();
            _glue.Init(new KSModule());
        }

#pragma warning disable 1998
        public async Task Init()
#pragma warning restore 1998
        {
            Directory.SetCurrentDirectory(@"C:\Users\jak\Documents\GitHub\FunctionsAsWebProject\FunctionsAsWebProject\bin\Release\PublishOutput");
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
