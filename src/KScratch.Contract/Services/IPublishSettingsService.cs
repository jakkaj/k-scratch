using KScratch.Entity.AzureEntities;

namespace KScratch.Contract.Services
{
    public interface IPublishSettingsService
    {
        PublishData LoadPublishProfile(string profileFilePath);
    }
}