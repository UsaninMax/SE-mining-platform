using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.Trades
{
    public interface ITradesDownloader
    {
        void Download(Instrument instrument);
    }
}
