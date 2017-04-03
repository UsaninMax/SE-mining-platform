using System;

namespace TradePlatform.StockDataDownload.model
{
    public class Instrument
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Id { get; set; }
        public string MarketId { get; set; }
        public string Path { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
