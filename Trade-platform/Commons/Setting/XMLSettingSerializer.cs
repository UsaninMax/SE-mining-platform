using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace TradePlatform.Commons.Setting
{
    public class XmlSettingSerializer : ISettingSerializer
    {
        public T Deserialize<T>(string path)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(T));
            FileStream fs = new FileStream(path, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            T desObject = (T)dcs.ReadObject(reader);
            reader.Close();
            fs.Close();
            return desObject;
        }

        public void Serialize<T>(T serObject, string path)
        {
            DataContractSerializer dcs = new DataContractSerializer(serObject.GetType());
            FileStream writer = new FileStream(path, FileMode.Create);
            dcs.WriteObject(writer, serObject);
            writer.Close();
        }
    }
}
