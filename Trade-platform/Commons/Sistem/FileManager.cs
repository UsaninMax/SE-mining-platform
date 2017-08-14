using System;
using System.IO;
using System.Text;
using TradePlatform.Commons.Loggers;

namespace TradePlatform.Commons.Sistem
{
    public class FileManager : IFileManager
    {
        public void CreateFile(string text, string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                using (FileStream fs = File.Create(path))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(text);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log.Error(ex);
            }
        }

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
