using System.Collections.Generic;
using System.IO;

namespace Toffee.Core.Infrastructure
{
    public interface IFilesystem
    {
        IEnumerable<string> ReadAllLines(string path);
        string ReadAllText(string path);
        void WriteAllLines(string path, IEnumerable<string> lines);
        bool FileExists(string path);
        bool DirectoryExists(string path);
        FileInfo[] GetFiles(string directoryPath);
        string GetFileByPartialName(string directoryPath, string partialFileName);
        void CreateDirectory(string path);
        void CreateFile(string path);
        void AppendLine(string filePath, string line);
        IEnumerable<FileInfo> GetFilesByExtensionRecursively(string path, string extension);
        void AppendLines(string path, IEnumerable<string> lines);
    }
}
