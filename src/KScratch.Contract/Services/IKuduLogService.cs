using System;
using System.Threading;
using System.Threading.Tasks;
using KScratch.Entity.Kudu;

namespace KScratch.Contract.Services
{
    public interface IKuduLogService
    {
        void StopLog();

        Task<bool> StartLog(KuduSiteSettings settings, 
            Action<string> callback);
    }
}