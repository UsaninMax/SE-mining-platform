using System.Threading;
using System.Threading.Tasks;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.DataServices.Trades
{
    interface IInstrumentDownloadService
    {
        void Download(Instrument instrument, CancellationToken cancellationToken);
        void Delete(Instrument instrument, Task download, CancellationTokenSource cancellationTokenSource);
        bool CheckFiles(Instrument instrument);
    }
}
