using System.Collections.Generic;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Setting;
using TradePlatform.Commons.Sistem;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.Serialization
{
    public class XmlInstrumentStorage : IInstrumentsStorage
    {
        private static string Path => "Settings\\FinamInstruments.xml";
        private readonly IFileManager _fileManager;
        private readonly ISettingSerializer _serializer;

        public XmlInstrumentStorage()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
            _serializer = ContainerBuilder.Container.Resolve<ISettingSerializer>();
        }

        public void Store(IEnumerable<Instrument> instruments)
        {
            CreateRoot();
            _serializer.Serialize(instruments, Path);
        }

        private void CreateRoot()
        {
            _fileManager.CreateFolder("Settings");
        }

        public IList<Instrument> ReStore()
        {
            if (!_fileManager.IsFileExist(Path))
            {
                return new List<Instrument>();
            }

            return _serializer.Deserialize<IList<Instrument>>(Path);
        }
    }
}
