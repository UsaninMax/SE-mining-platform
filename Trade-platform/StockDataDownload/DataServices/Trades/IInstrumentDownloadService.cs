using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    interface IInstrumentDownloadService
    {
        void Execute(Instrument instrument);
    }
}
