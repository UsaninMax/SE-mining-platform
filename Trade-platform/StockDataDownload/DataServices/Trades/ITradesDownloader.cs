using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    public interface ITradesDownloader
    {
        void Download(Instrument instrument);
    }
}
