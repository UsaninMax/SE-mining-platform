using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    internal interface ITradesDownloader
    {
        void Download(Instrument instrument);
    }
}
