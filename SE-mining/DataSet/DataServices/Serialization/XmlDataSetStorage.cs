﻿using System.Collections.Generic;
using Microsoft.Practices.Unity;
using SEMining.Commons.Setting;
using SEMining.Commons.Sistem;
using SEMining.DataSet.Models;

namespace SEMining.DataSet.DataServices.Serialization
{
    public class XmlDataSetStorage : IDataSetStorage
    {
        private static string Path => "Settings\\DataSets.xml";
        private readonly IFileManager _fileManager;
        private readonly ISettingSerializer _serializer;

        public XmlDataSetStorage()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
            _serializer = ContainerBuilder.Container.Resolve<ISettingSerializer>();
        }

        public void Store(IEnumerable<DataSetItem> dataSets)
        {
            CreateRoot();
            _serializer.Serialize(dataSets, Path);
        }

        private void CreateRoot()
        {
            _fileManager.CreateFolder("Settings");
        }

        public IEnumerable<DataSetItem> ReStore()
        {
            if (!_fileManager.IsFileExist(Path))
            {
                return new List<DataSetItem>();
            }

            return _serializer.Deserialize<IEnumerable<DataSetItem>>(Path);
        }
    }
}
