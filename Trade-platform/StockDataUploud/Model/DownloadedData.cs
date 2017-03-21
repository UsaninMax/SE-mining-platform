using System;
using TradePlatform.StockDataUploud.Model;

namespace TradePlatform.StockDataUploud.model
{
    public class DownloadedData
    {
        public string Instrument { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public float Step { get; set; }
        public DownloadStatus Status { get; set; }
    }
}
