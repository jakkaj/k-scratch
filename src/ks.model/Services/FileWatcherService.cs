using ks.model.Contract.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ks.model.Services
{
    public class FileWatcherService : IFileWatcherService
    {
        private FileSystemWatcher _watcher;

        System.Threading.Timer _timer;

        private List<string> _filesChangedList = new List<string>();

        static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

       
        readonly ILocalLogService _localLogService;

        public FileWatcherService(ILocalLogService logService)
        {
            this._localLogService = logService;       
        }

        public void Watch(string path, Func<string, Task> callback)
        {
            var baseDir = Directory.GetCurrentDirectory();

            _watcher = new FileSystemWatcher(baseDir);
            _watcher.IncludeSubdirectories = true;

            _watcher.Changed += _watcher_Changed;
            _watcher.Created += _watcher_Changed;

            _watcher.EnableRaisingEvents = true;

            _localLogService.LogInfo($"Monitoring {baseDir} for changes");

            _timer = new Timer(new TimerCallback(_monitorCallback), callback, 1000, 2000);
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        async void _monitorCallback(object data)
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            var callback = data as Func<string, Task>;

            await _semaphore.WaitAsync();

            try
            {
                foreach (var f in _filesChangedList)
                {
                    _localLogService.LogInfo($"[Sending] {f.Replace(Directory.GetCurrentDirectory(), "")}");
                    if (callback != null) await callback(f);
                }
            }
            catch (Exception ex)
            {
                _localLogService.LogError($"Error: exception in _monitorCallback: {ex.Message}");
            }
            finally
            {
                _filesChangedList.Clear();
                _semaphore.Release();
            }
        }

        public bool Validate(string fullPath)
        {
            //we only want to send files that are in folders. 
            var pathSubs = fullPath.Replace(Directory.GetCurrentDirectory(), "").Trim(Path.DirectorySeparatorChar);

            if (pathSubs.IndexOf(Path.DirectorySeparatorChar) == -1)
            {
                return false;
            }

            if (pathSubs.IndexOf("~", StringComparison.Ordinal) != -1)
            {
                return false;
            }

            var fn = Path.GetFileName(fullPath);
            if (fullPath.Contains("/.") ||
                fullPath.Contains("\\.") ||
                fn.StartsWith(".", StringComparison.Ordinal)
                )
            {
                return false;
            }

            return true;
        }

        private async void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var f = e.FullPath;

            if (!Validate(f))
            {
                return;
            }

            await _semaphore.WaitAsync();

            if (!_filesChangedList.Contains(f))
            {
                _filesChangedList.Add(f);
            }

            _semaphore.Release();
        }
    }
}
