namespace ks.model.Contract.Services
{
    public interface ICommandProcessorService
    {
        int Process(string[] args);
    }
}