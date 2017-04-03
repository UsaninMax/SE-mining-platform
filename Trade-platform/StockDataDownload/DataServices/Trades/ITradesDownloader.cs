using System.Threading.Tasks;
using TradePlatform.StockDataDownload.model;


namespace TradePlatform.StockDataDownload.Services
{
    interface ITradesDownloader
    {
        void Download(Instrument instrument);
    }
}
