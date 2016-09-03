using KScratch.Contract.Services;
using KScratch.Entity.AzureEntities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KScratch.Portable.Services
{
    public class PublishSettingsService : IPublishSettingsService
    {
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
                return result;
            }
        }
    }
}
