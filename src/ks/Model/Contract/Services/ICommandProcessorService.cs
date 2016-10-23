namespace ks.Model.Services
{
    public interface ICommandProcessorService
    {
        int Process(string[] args);
    }
}