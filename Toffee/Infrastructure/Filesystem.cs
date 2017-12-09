using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Toffee.Infrastructure
{
    public class Filesystem : IFilesystem
    {
        private static readonly Encoding Encoding = Encoding.UTF8;

        public IEnumerable<string> ReadAllLines(string path)
        {
            return File.ReadAllLines(path, Encoding);
        }

        public string ReadAllText(string path)
        {
            return File.ReadAllText(path, Encoding);
        }

        public void WriteAllLines(string path, IEnumerable<string> lines)
        {
            File.WriteAllLines(path, lines, Encoding);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public FileInfo[] GetFiles(string directoryPath)
        {
            return Directory.GetFiles(directoryPath).Select(file => new FileInfo(file)).ToArray();
        }

        public string GetFileByPartialName(string directoryPath, string partialFileName)
        {
            return Directory.GetFiles(directoryPath).FirstOrDefault(file => file.Contains(partialFileName));
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public void CreateFile(string path)
        {
            File.Create(path);
        }

        public void AppendLine(string filePath, string line)
        {
            File.AppendAllLines(filePath, new []{line});
        }

        public IEnumerable<FileInfo> GetFilesByExtensionRecursively(string path, string extension)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists)
            {
                return directory.GetFiles(extension, SearchOption.AllDirectories);
            }

            throw new DirectoryNotFoundException($"The directory {path} does not exist");
        }
    }
}