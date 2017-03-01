using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ks.model.Contract.Services;
using ks.model.Entity.AzureEntities;
using ks.model.Entity.Enum;
using ks.model.Entity.Kudu;

namespace ks.model.Services
{
    public class PublishSettingsService : IPublishSettingsService
    {
        private readonly ILocalLogService _localLogService;

        /// <summary>
        /// Data of the loaded publish profile. 
        /// </summary>
        public PublishData PublishData { get; private set; }

        public PublishSettingsService(ILocalLogService localLogService)
        {
            _localLogService = localLogService;
        }

        /// <summary>
        /// Discover the publish profile, load it and find/load the web deploy option
        /// </summary>
        /// <returns>True if discovery was successful. </returns>
        public KuduSiteSettings AutoLoadPublishProfile()
        {
            var discoveredProfile = DiscoverPublishProfile();

            if (discoveredProfile == null)
            {
                return null;
            }

            _localLogService.LogInfo($"Profile settings: {discoveredProfile}");
            PublishData = LoadPublishProfile(discoveredProfile);

            if (PublishData == null)
            {
                return null;
            }

            //find the web site settings and return

            var kuduSettings = GetSettingsByPublishMethod(PublishMethods.MSDeploy);
            return kuduSettings;
        }

        /// <summary>
        /// Looks in the current folder and all parent paths for a publish file. 
        /// </summary>
        /// <returns>File path of the profile</returns>
        public string DiscoverPublishProfile()
        {
            var currentPath = Directory.GetCurrentDirectory();
            var result = _recursePath(currentPath);
            return result;
        }

        /// <summary>
        /// Scan up the directory structure until a *.PublishSettings
        /// </summary>
        /// <param name="path">Current path to scan</param>
        /// <returns></returns>
        string _recursePath(string path)
        {
            var currentDir = new DirectoryInfo(path);

            foreach (var file in currentDir.GetFiles("*.PublishSettings"))
            {
                return file.FullName;
            }

            var parent = currentDir.Parent;

            if (parent == null)
            {
                return null;
            }

            return _recursePath(parent.FullName);
        }

        /// <summary>
        /// Loads and parses the publish profile. Data is store in PublishData 
        /// </summary>
        /// <param name="profileFilePath">Path to the publish profile</param>
        /// <returns>PublishData object containing the loaded publish profile</returns>

        public PublishData LoadPublishProfile(string profileFilePath)
        {
            
            XmlSerializer serializer =
                new XmlSerializer(typeof(PublishData));

            if (!File.Exists(profileFilePath))
            {
                return null;
            }

            using (Stream reader = new FileStream(profileFilePath, FileMode.Open))
            {
                var result = (PublishData)serializer.Deserialize(reader);
                PublishData = result;
                return result;
            }
        }

        /// <summary>
        /// Gets the settings from the publish settings file by method and converts it to a KuduSiteSettings object
        /// </summary>
        public KuduSiteSettings GetSettingsByPublishMethod(string method)
        {
            var pubSetting = PublishData.PublishProfile.FirstOrDefault(
                _ => _.PublishMethod == method);

            if (pubSetting == null)
            {
                return null;
            }

            return _pub2Kudu(pubSetting);
        }

        /// <summary>
        /// Gets the settings from the publish settings file by profile name and converts it to a KuduSiteSettings object
        /// </summary>
        public KuduSiteSettings GetSettingsByProfileName(string profileName)
        {
            var pubSetting = PublishData.PublishProfile.FirstOrDefault(
                _ => _.ProfileName == profileName);

            if (pubSetting == null)
            {
                return null;
            }

            return _pub2Kudu(pubSetting);
        }

        KuduSiteSettings _pub2Kudu(PublishProfile pubSetting)
        {
            return new KuduSiteSettings
            {
                ApiUrl = pubSetting.PublishUrl,
                LiveUrl = pubSetting.DestinationAppUrl,
                Password = pubSetting.UserPWD,
                SiteName = pubSetting.MsdeploySite,
                UserName = pubSetting.UserName,
                ProfileName = pubSetting.ProfileName
            };
        }
    }
}
