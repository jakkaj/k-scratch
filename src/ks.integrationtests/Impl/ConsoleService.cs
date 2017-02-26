using System;
using System.Collections.Generic;
using System.Text;
using ks.model.Contract.Services;

namespace ks.Impl
{
    public class ConsoleService : IConsoleService
    {
        public void SetWarning()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }

        public void SetError()
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }

        public void SetGood()
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public void SetNormal()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void SetInformation()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
        }
    }
}
