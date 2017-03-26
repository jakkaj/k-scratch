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
    public class FunctionSettingsTests :TestBase
    {
        [TestMethod]
        public async Task TestGetFunctionSettings()
        {
            await Init();

            var funcService = Resolve<IFunctionSettingsService>();

            var funcSettings = await funcService.GetFunctionSettings();

            Assert.IsNotNull(funcSettings);
        }
    }
}
