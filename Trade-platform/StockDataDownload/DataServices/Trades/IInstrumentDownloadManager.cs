using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.DataServices
{
    interface IInstrumentDownloadManager
    {
        bool Execute(Instrument instrument);
    }
}
