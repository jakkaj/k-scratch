using System;
using System.Threading.Tasks;

namespace ks.model.Contract.Services
{
    public interface IFileWatcherService
    {
        void Watch(string path, Func<string, Task> callback);
        bool Validate(string fullPath);
    }
}