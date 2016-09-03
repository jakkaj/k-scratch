using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KScratch.Contract.Services;
using Xunit;

namespace KScratch.Tests.Tests
{
    public class ProfileTests : TestBase
    {
        [Theory]
        [InlineData("C:\\Users\\jakka\\Downloads\\functionplayground.PublishSettings", true)]
        public void ReadProfileTest(string fileName, bool expectedResult)
        {
            var publishSettingsService = Resolve<IPublishSettingsService>();

            var loaded = publishSettingsService.LoadPublishProfile(fileName);

            if (expectedResult)
            {
                Assert.NotNull(loaded);
            }
            else
            {
                Assert.Null(expectedResult);
            }
        }
    }
}
