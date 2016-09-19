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
        public void ReadProfile(string fileName, bool expectedResult)
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

        [Theory]
        [InlineData("C:\\Users\\jakka\\Downloads\\functionplayground.PublishSettings",
            "functionplayground - Web Deploy")]
        public void GetKuduSiteSettings(string fileName, string profileName)
        {
            var publishSettingsService = Resolve<IPublishSettingsService>();

            var loaded = publishSettingsService.LoadPublishProfile(fileName);
            Assert.NotNull(loaded);

            var kuduSettings = publishSettingsService.GetSettingsByProfileName(profileName);

            Assert.NotNull(kuduSettings);

            Assert.True(kuduSettings.ProfileName == profileName);
        }
    }
}
