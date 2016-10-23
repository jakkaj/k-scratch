using System;
using ks.Model.Contract;
using ks.Model.Contract.Services;

namespace ks.Model.Services
{
    public class LocalLogService : ILocalLogService
    {
        public void Log(string output)
        {
            Console.WriteLine(output);
        }
    }
}
