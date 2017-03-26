using System.Threading.Tasks;
using ks.model.Entity.Kudu;
using System.Collections.Generic;

namespace ks.model.Services
{
    public interface IFunctionSettingsService
    {
        Task<List<FunctionSettings>> GetFunctionSettings();
    }
}