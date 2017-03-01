namespace ks.model.Contract.Services
{
    public interface ILocalLogService
    {
        void Log(string output);
        void LogError(string output);
        void LogSuccess(string output);
        void LogWarning(string output);
        void LogInfo(string output);
    }
}