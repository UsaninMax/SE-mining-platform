using System.Threading;
using TradePlatform.StockDataDownload.model;

namespace TradePlatform.StockDataDownload.Services
{
    class DownloadFinamInstrument : IDownloadInstrument
    {
        public bool download(Instrument instrument)
        {
            Thread.Sleep(5000);
            return true;
        }
    }
}
