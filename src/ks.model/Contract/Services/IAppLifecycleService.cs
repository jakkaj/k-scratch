namespace ks.model.Contract.Services
{
    public interface IAppLifecycleService
    {
        (int, bool) Init(string[] args);
    }
}