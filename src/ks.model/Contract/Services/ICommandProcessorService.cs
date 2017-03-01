namespace ks.model.Contract.Services
{
    public interface ICommandProcessorService
    {
        (int, bool) Process(string[] args);
    }
}