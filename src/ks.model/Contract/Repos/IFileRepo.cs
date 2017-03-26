using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ks.model.Contract.Repos
{
    public interface IFileRepo
    {
        string PathOffset(string path);
        Task<bool> CopyFile(string source, string targetDirectory);
        Task<int> CopyFolder(string source, string target, List<string> allowedExtensions);
        Task DeleteDirectory(string path);
        Task DeleteFile(string path);
        Task DeleteFiles(string path);
        Task<bool> DirectoryExists(string directory);
        Task<bool> FileExists(string filePath);
        Task<bool> FolderExists(string folderPath);
        string GetFileNameComponent(string fullPath);
        Task<List<string>> GetFiles(string path, List<string> extensions = null);
        string GetHash(byte[] bytes);
        Task<string> GetHash(string fileName);
        
        Task<string> GetParentFolder(string path);
        string GetPathSeparator();
        Task<bool> HasFiles(string path);
        Task<bool> MoveFile(string source, string target);
        Task<byte[]> ReadBytes(string file);
        Task<Stream> ReadStream(string file);
        Task<string> ReadText(string file);
        Task<bool> Write(string file, byte[] data);
        Task<bool> Write(string file, string text);
    }
}