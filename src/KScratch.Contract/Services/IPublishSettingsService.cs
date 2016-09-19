using KScratch.Entity.AzureEntities;
using KScratch.Entity.Kudu;

namespace KScratch.Contract.Services
{
    public interface IPublishSettingsService
    {
        PublishData LoadPublishProfile(string profileFilePath);

        /// <summary>
        /// Gets the settings from the publish settings file and converts it to a KuduSiteSettings object
        /// </summary>
        KuduSiteSettings GetSettingsByProfileName(string profileName);

        /// <summary>
        /// Gets the settings from the publish settings file by method and converts it to a KuduSiteSettings object
        /// </summary>
        KuduSiteSettings GetSettingsByPublishMethod(string method);
    }
}