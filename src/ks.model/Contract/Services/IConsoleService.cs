namespace ks.model.Contract.Services
{
    public interface IConsoleService
    {
        void SetWarning();
        void SetError();
        void SetGood();
        void SetNormal();
        void SetInformation();
    }
}