using ks.model.Services;
using ks.unittests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ks.integrationtests.Tests
{
    [TestClass]
    public class TestFuncTests : TestBase
    {
        [TestMethod]
        public async Task TestCallHttp()
        {
            await Init();
            var tester = Resolve<ITestService>();
            var p = Resolve<IParamService>();

            p.Add("key", "r5kV9oZQLV5aPXzNhpX21GKTi5riIZFU3hW2TJaQB/ux/uq0tKzKjw==");

            var result2 = await tester.RunTest(2);
            var result = await tester.RunTest(3);
            
            Assert.IsTrue(result);
        }
    }
}
