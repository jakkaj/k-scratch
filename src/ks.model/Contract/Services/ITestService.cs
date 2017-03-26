using System.Threading.Tasks;

namespace ks.model.Services
{
    public interface ITestService
    {
        Task<bool> RunTest(int testNumber);
    }
}