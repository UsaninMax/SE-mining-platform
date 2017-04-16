using System.IO;

namespace TradePlatform.Commons.Sistem
{
    public class FileManager : IFileManager
    {
        public void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void DeleteFolder(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        public bool IsFileExist(string path)
        {
            return File.Exists(path);
        }

        public void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start(path);
        }
    }
}
