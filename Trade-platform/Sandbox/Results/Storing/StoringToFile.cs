using Castle.Core.Internal;
using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.Commons.Sistem;
using Microsoft.Practices.Unity;
using System.Text;
using System.IO;

namespace TradePlatform.Sandbox.Results.Storing
{
    public class StoringToFile : IResultStoring
    {
        public static readonly string  STORAGE_FOLDER = "RESULT_STORAGE";
        private readonly IFileManager _fileManager;
        public StoringToFile()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
            CreateRoot();
        }

        public void Store(IEnumerable<Dictionary<string, string>> data, string separator)
        {
            if(data.IsNullOrEmpty())
            {
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeader(data.First(), separator));
            sb.Append(Environment.NewLine);
            data.ForEach(line =>
            {
                sb.Append(GetBody(line, separator));
                sb.Append(Environment.NewLine);
            });

            _fileManager.CreateFile(sb.ToString(), STORAGE_FOLDER + "\\" + Path.GetRandomFileName());
        }

        private String GetHeader(Dictionary<string, string> header, string separator)
        {
            return string.Join(separator, header.Keys.ToArray());
        }

        private String GetBody(Dictionary<string, string> body, string separator)
        {
            return string.Join(separator, body.Values.ToArray());
        }

        private void CreateRoot()
        {
            if(!_fileManager.IsDirectoryExist(STORAGE_FOLDER))
            {
                _fileManager.CreateFolder(STORAGE_FOLDER);
            }
        }
    }
}
