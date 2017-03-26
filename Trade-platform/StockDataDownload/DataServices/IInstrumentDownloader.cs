using System.Threading;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.Services
{
    interface IInstrumentDownloader
    {
        bool Download(Instrument instrument, CancellationToken ct);
    }
}
