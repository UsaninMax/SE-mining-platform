using System;
using System.Collections.Generic;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Serialization
{
    class XmlInstrumentSerializer : IInstrumentsSerializer
    {
        private static string Path => "Instruments";

        public void Serialize(IEnumerable<Instrument> instruments)
        {
            throw new System.NotImplementedException();
        }

        public IList<Instrument> Deserialize()
        {
            return new List<Instrument>() {new Instrument.Builder().WithFrom(DateTime.Now).WithTo(DateTime.Now).WithName("DDDD").WithCode("FFFF").WithId("332").Build() };
        }
    }
}
