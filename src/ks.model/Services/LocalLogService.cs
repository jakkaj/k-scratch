using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ks.model.Contract.Services;

namespace ks.model.Services
{
    public class LocalLogService : ILocalLogService
    {
        private readonly IConsoleService _consoleService;
        private List<string> _errors = new List<string> { "error", "failed"};
        private List<string> _good = new List<string> { "succeeded" };
        private List<string> _warn = new List<string> { "warn" };
        private List<string> _information = new List<string> { "function completed", "reloading", "function started" };

        public LocalLogService(IConsoleService consoleService)
        {
            _consoleService = consoleService;
        }

        bool _check(string output, List<string> checkThings)
        {
            var outputLower = output.ToLower();
            return checkThings.Select(thing => thing.ToLower()).Any(thingLower => outputLower.Contains(thingLower));
        }

        public void LogError(string output)
        {
            _consoleService.SetError();
            Console.WriteLine("");
            Log(output);
            Console.WriteLine("");
        }

        public void LogSuccess(string output)
        {
            _consoleService.SetGood();
            Log(output);
        }

        public void LogWarning(string output)
        {
            _consoleService.SetWarning();
            Log(output);
        }

        public void LogInfo(string output)
        {
            _consoleService.SetInformation();
            Log($"- {output}");
        }

        public void Log(string output)
        {

            if (!output.StartsWith(">") && !output.StartsWith("-"))
            {
                output = " " + output;
            }

            if (_check(output, _information))
            {
                _consoleService.SetInformation();
            }


            if (_check(output, _good))
            {
                _consoleService.SetGood();
            }

            if (_check(output, _warn))
            {
                _consoleService.SetWarning();
            }


            if (_check(output, _errors))
            {
                _consoleService.SetError();
            }

            Console.WriteLine(output);
            _consoleService.SetNormal();

#if DEBUG
            Debug.WriteLine(output);
#endif
        }
    }
}
