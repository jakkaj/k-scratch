using System.Threading.Tasks;

namespace ks.model.Contract.Services
{
    public interface IKuduLogService
    {
        void StopLog();

        Task<bool> StartLog();
    }
}