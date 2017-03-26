using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ks.Glue;
using ks.model.Contract.Services;
using ks.model.Glue;

using ks.model.Contract.Repos;
using ks.model.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        public ITestService TestService
        {
            get
            {
                return Resolve<ITestService>();
            }
        }

        public IFileRepo FileRepo
        {
            get
            {
                return Resolve<IFileRepo>();
            }
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

        /// <summary>
        /// Thanks to Stephen Cleary http://stackoverflow.com/questions/13634779/assert-throwsexception-in-async-function-unit-test
        /// </summary>
        /// <typeparam name="TException"></typeparam>
        /// <param name="action"></param>
        /// <param name="allowDerivedTypes"></param>
        /// <returns></returns>
        public async Task ThrowsAsync<TException>(Func<Task> action, bool allowDerivedTypes = true)
        {
            try
            {
                await action();
                Assert.Fail("Delegate did not throw expected exception " + typeof(TException).Name + ".");
            }
            catch (Exception ex)
            {
                if (allowDerivedTypes && !(ex is TException))
                    Assert.Fail("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " or a derived type was expected.");
                if (!allowDerivedTypes && ex.GetType() != typeof(TException))
                    Assert.Fail("Delegate threw exception of type " + ex.GetType().Name + ", but " + typeof(TException).Name + " was expected.");
            }
        }
    }
}
