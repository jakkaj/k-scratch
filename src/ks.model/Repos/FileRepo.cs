using ks.model.Contract.Repos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ks.model.Repos
{
    public class FileRepo : IFileRepo
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        private static object _writeLock = new object();

        public string PathOffset(string path)
        {
            path = path.Trim('\\').Trim('/');

            path = path.Replace('\\', Path.DirectorySeparatorChar);
            path = path.Replace('/', Path.DirectorySeparatorChar);

            var baseDir = Path.Combine(Directory.GetCurrentDirectory(), path);

            return baseDir;
        }

        public async Task<string> GetHash(string fileName)
        {
            var f = _getFile(fileName);

            if (!f.Exists)
            {
                return null;
            }

            var hashSource = f.Length.ToString() + f.Name;

            var hMade = _createSHA1(Encoding.UTF8.GetBytes(hashSource));

            return hMade;
        }

        public string GetHash(byte[] bytes)
        {
            return _createSHA1(bytes);
        }

        string _createSHA1(byte[] bytes)
        {

            var sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(bytes);

            return _hexStringFromBytes(hashBytes);
        }

        string _hexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }        

        public async Task<bool> Write(string file, byte[] data)
        {
            var f = _getFile(file);
            try
            {
                File.WriteAllBytes(f.FullName, data);

            }
            catch (Exception ex)
            {
                //erm... ignore?
                return false;
            }
            return true;
        }

        public async Task<bool> Write(string file, string text)
        {
            var f = _getFile(file);
            lock (_writeLock)
            {
                File.WriteAllText(f.FullName, text);
            }

            return true;
        }

        public async Task<Stream> ReadStream(string file)
        {
            var f = _getFile(file);
            if (!f.Exists)
            {
                return null;
            }

            var fStream = f.OpenRead();

            return fStream;
        }

        public async Task<byte[]> ReadBytes(string file)
        {
            var f = _getFile(file, false);
            if (!f.Exists)
            {
                return null;
            }

            return File.ReadAllBytes(f.FullName);
        }


        public async Task<string> ReadText(string file)

        {
            var f = _getFile(file, false);

            if (!f.Exists)
            {
                return null;
            }

            return File.ReadAllText(f.FullName);
        }

        DirectoryInfo _getDir(string dir, bool createPath = true)
        {
            var d = new DirectoryInfo(dir);
            if (!d.Exists && createPath)
            {
                d.Create();
            }

            return d;
        }

        FileInfo _getFile(string file, bool createPath = true)
        {
            var f = new FileInfo(file);
            if (!f.Directory.Exists && createPath)
            {
                f.Directory.Create();
            }
            return f;
        }

        public async Task<string> GetParentFolder(string path)
        {
            return Path.GetDirectoryName(path);
        }


        public async Task<bool> FolderExists(string folderPath)
        {
            if (folderPath == null)
            {
                return false;
            }
            var f = new DirectoryInfo(folderPath);
            return f.Exists;
        }

        public async Task<bool> FileExists(string filePath)
        {
            if (filePath == null)
            {
                return false;
            }
            var f = new FileInfo(filePath);
            return f.Exists;
        }

        public string GetFileNameComponent(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public string GetPathSeparator()
        {
            return Path.DirectorySeparatorChar.ToString();
        }

        public async Task<bool> DirectoryExists(string directory)
        {
            var d = new DirectoryInfo(directory);
            return d.Exists;
        }

        public async Task<bool> MoveFile(string source, string target)
        {
            var fSource = new FileInfo(source);

            if (!fSource.Exists)
            {
                return false;
            }

            fSource.MoveTo(target);

            return true;
        }

        public async Task<bool> CopyFile(string source, string targetDirectory)
        {
            var fSource = new FileInfo(source);
            var fTarget = new DirectoryInfo(targetDirectory);

            if (!fSource.Exists)
            {
                return false;
            }

            if (!fTarget.Exists)
            {
                fTarget.Create();
            }

            fSource.CopyTo(Path.Combine(fTarget.FullName, fSource.Name), true);

            return true;
        }

        public async Task<int> CopyFolder(string source, string target, List<string> allowedExtensions)
        {
            var dSource = new DirectoryInfo(source);
            var dTarget = new DirectoryInfo(target);

            if (!dSource.Exists)
            {
                dSource.Create();
            }
            if (!dTarget.Exists)
            {
                dTarget.Create();
            }

            var sourceFiles = dSource.GetFiles();

            var allowedLower = allowedExtensions.Select(_ => _.ToLower()).ToList();

            foreach (var f in sourceFiles)
            {
                if (!allowedLower.Contains(f.Extension.ToLower()))
                {
                    continue;
                }
                f.CopyTo(Path.Combine(dTarget.FullName, f.Name));
            }
            return sourceFiles.Length;
        }
       
        public async Task<bool> HasFiles(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            return dirInfo.GetFiles().Length > 0;
        }

        public async Task DeleteDirectory(string path)
        {
            var dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                return;
            }

            await DeleteFiles(path);
            dirInfo.Delete(true);

        }
        public async Task DeleteFiles(string path)
        {
            var dirInfo = new DirectoryInfo(path);

            if (!dirInfo.Exists)
            {
                return;
            }

            foreach (var fileInfo in dirInfo.GetFiles())
            {
                fileInfo.Delete();
            }
        }

        public async Task DeleteFile(string path)
        {
            var f = _getFile(path);
            if (!f.Exists)
            {
                return;
            }
            f.Delete();
        }

        public async Task<List<string>> GetFiles(string path, List<string> extensions = null)
        {
            var dirInfo = new DirectoryInfo(path);

            var l = dirInfo.GetFiles().Select(_ => _.FullName).ToList();

            var lResult = new List<string>();

            if (extensions == null)
            {
                lResult = l;
            }
            else
            {
                foreach (var lFile in l)
                {
                    var extension = Path.GetExtension(lFile);

                    lResult.AddRange(from ext in extensions
                                     where ext.ToLower().IndexOf(extension.ToLower()) != -1
                                     select lFile);
                }
            }

            foreach (var child in dirInfo.GetDirectories())
            {
                lResult.AddRange(await GetFiles(child.FullName, extensions));
            }

            return lResult;

        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    }
}
