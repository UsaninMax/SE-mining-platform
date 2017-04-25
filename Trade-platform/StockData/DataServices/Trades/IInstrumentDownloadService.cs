using System.Threading;
using System.Threading.Tasks;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.DataServices.Trades
{
    public interface IInstrumentDownloadService
    {
        void Download(Instrument instrument, CancellationToken cancellationToken);
        void SoftDownload(Instrument instrument, CancellationToken cancellationToken);
        void Delete(Instrument instrument, Task download, CancellationTokenSource cancellationTokenSource);
        bool CheckFiles(Instrument instrument);
    }
}
