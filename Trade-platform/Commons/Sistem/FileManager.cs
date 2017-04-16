using System;

namespace TradePlatform.Commons.Sistem
{
    public class FileManager : IFileManager
    {
        public void OpenFolder(string path)
        {
            System.Diagnostics.Process.Start(path);
        }
    }
}
