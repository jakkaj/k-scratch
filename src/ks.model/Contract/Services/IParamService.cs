namespace ks.model.Services
{
    public interface IParamService
    {
        void Add(string name, string value);
        string Get(string name);
    }
}