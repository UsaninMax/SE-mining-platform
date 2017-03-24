using System.Threading;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.Services
{
    interface IDownloadInstrument
    {
        bool download(Instrument instrument, CancellationToken ct);
    }
}
