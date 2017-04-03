using System.Net;

namespace TradePlatform.StockDataDownload.Services
{
    class FinamSecuritiesInfoDownloader : ISecuritiesInfoDownloader
    {
        private string _url = "https://www.finam.ru/cache/icharts/icharts.js";
        public string Download() 
        {
            return new WebClient().DownloadString(_url);
        }
    }
}
