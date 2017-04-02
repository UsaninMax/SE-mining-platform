using System;

namespace TradePlatform.StockDataDownload.Models
{
    class Trade
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }
    }
}
