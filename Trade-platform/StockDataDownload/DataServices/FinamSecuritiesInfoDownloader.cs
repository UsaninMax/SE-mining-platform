
using System.Threading;

namespace TradePlatform.StockDataDownload.Services
{
    class FinamSecuritiesInfoDownloader : ISecuritiesInfoDownloader
    {
        public bool Download()
        {
            Thread.Sleep(12000);
            return true;
        }
    }
}
