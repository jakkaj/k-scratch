using ks.model.Entity.AzureEntities;
using ks.model.Entity.Kudu;

namespace ks.model.Contract.Services
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

        /// <summary>
        /// Discover the publish profile, load it and find/load the web deploy option
        /// </summary>
        /// <returns>True if discovery was successful. </returns>
        KuduSiteSettings AutoLoadPublishProfile();
    }
}