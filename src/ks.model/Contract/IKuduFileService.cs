using System.Threading.Tasks;

namespace ks.model.Contract
{
    public interface IKuduFileService
    {
        Task ListFiles();
        Task SendFile(string offsetFile);
        void Monitor();
    }
}