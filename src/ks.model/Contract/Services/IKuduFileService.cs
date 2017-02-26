using System.Threading.Tasks;

namespace ks.model.Contract.Services
{
    public interface IKuduFileService
    {
        Task ListFiles();
        Task SendFile(string offsetFile);
        void Monitor();
        Task PullFiles();
    }
}