using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.DataServices
{
    interface IInstrumentDownloadService
    {
        bool Execute(Instrument instrument);
    }
}
