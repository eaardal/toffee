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
            var lines = new List<string>();

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(stream, Encoding))
                {
                    lines.Add(streamReader.ReadLine());
                }
            }

            return lines;
        }

        public string ReadAllText(string path)
        {
            //return Retry.Operation(() => File.ReadAllText(path, Encoding));
            
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(stream, Encoding))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public void WriteAllLines(string path, IEnumerable<string> lines)
        {
            //Retry.Operation(() => File.WriteAllLines(path, lines, Encoding));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
            {
                using (var streamWriter = new StreamWriter(stream, Encoding))
                {
                    foreach (var line in lines)
                    {
                        streamWriter.WriteLine(line);
                    }
                }
            }
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
            Retry.Operation(() => File.Create(path));
        }

        public void AppendLine(string path, string line)
        {
            //Retry.Operation(() => File.AppendAllLines(filePath, new []{line}));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
            {
                using (var streamWriter = new StreamWriter(stream, Encoding))
                {
                    streamWriter.WriteLine(line);
                }
            }
        }

        public IEnumerable<FileInfo> GetFilesByExtensionRecursively(string path, string extension)
        {
            var directory = new DirectoryInfo(path);

            if (directory.Exists)
            {
                return directory.GetFiles($"*{extension}", SearchOption.AllDirectories);
            }

            throw new DirectoryNotFoundException($"The directory {path} does not exist");
        }

        public void AppendLines(string path, IEnumerable<string> lines)
        {
            //Retry.Operation(() => File.AppendAllLines(path, lines));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.Read))
            {
                using (var streamWriter = new StreamWriter(stream, Encoding))
                {
                    foreach (var line in lines)
                    {
                        streamWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}