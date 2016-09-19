using KScratch.Contract.Services;
using KScratch.Entity.AzureEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using KScratch.Entity.Kudu;

namespace KScratch.Portable.Services
{
    public class PublishSettingsService : IPublishSettingsService
    {
        public PublishData PublishData { get; private set; }

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
