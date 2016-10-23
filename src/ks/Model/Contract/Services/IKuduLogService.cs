using System;
using System.Threading.Tasks;
using ks.Model.Entity.Kudu;

namespace ks.Model.Contract.Services
{
    public interface IKuduLogService
    {
        void StopLog();

        Task<bool> StartLog();
    }
}