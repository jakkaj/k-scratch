using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

using KScratch.Portable.Glue;

namespace KScratch.Tests
{
    public class TestBase
    {
        private CoreGlue _glue;

        public TestBase()
        {
            _glue = new CoreGlue();
            _glue.Init();
        }

        public T Resolve<T>()
        {
            return _glue.Container.Resolve<T>();
        }
    }
}
