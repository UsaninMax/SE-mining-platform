using System;

namespace TradePlatform.StockDataDownload.model
{
    public class Instrument
    {
        public string Name { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
