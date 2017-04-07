using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Serialization
{
    class XmlInstrumentStorage : IInstrumentsStorage
    {
        private static string Root => "Settings";

        public void Store(IEnumerable<Instrument> instruments)
        {
            Serialize<IEnumerable<Instrument>>(instruments, "FinamInstruments.xml");
        }

        private void Serialize<T>(T serObject, string path)
        {
            CreateRoot();
            DataContractSerializer dcs = new DataContractSerializer(serObject.GetType());
            FileStream writer = new FileStream(Root + "\\" + path, FileMode.Create);
            dcs.WriteObject(writer, serObject);
            writer.Close();
        }

        private T Deserialize<T>(string path)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            FileStream fs = new FileStream(Root + "\\" + path, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            T desObject = (T)dcs.ReadObject(reader);
            reader.Close();
            fs.Close();
            return desObject;
        }

        private void CreateRoot()
        {
            if (!Directory.Exists(Root))
            {
                Directory.CreateDirectory(Root);
            }
        }

        public IList<Instrument> ReStore()
        {
            return Deserialize<IList<Instrument>>("FinamInstruments.xml");
        }
    }
}
