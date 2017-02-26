using System;
using System.Diagnostics;
using ks.model.Contract.Services;

namespace ks.model.Services
{
    public class LocalLogService : ILocalLogService
    {
        public void Log(string output)
        {
            Console.WriteLine(output);

#if DEBUG
              Debug.WriteLine(output);
#endif
        }
    }
}
