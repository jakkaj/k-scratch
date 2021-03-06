using System.Threading.Tasks;

namespace ks.model.Contract.Services
{
    public interface IKuduFileService
    {
        Task ListFiles();
        Task SendFile(string offsetFile);
        void Monitor();
        Task<bool> GetFiles(string subPath = null);
        Task<bool> UploadFiles(string subPath = null);
    }
}