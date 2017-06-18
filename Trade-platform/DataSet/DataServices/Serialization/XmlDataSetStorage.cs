using System.Collections.Generic;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Setting;
using TradePlatform.Commons.Sistem;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices.Serialization
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

        public IList<DataSetItem> ReStore()
        {
            if (!_fileManager.IsFileExist(Path))
            {
                return new List<DataSetItem>();
            }

            return _serializer.Deserialize<IList<DataSetItem>>(Path);
        }
    }
}
