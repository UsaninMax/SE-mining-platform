using System.Net;

namespace TradePlatform.StockData.DataServices.SecuritiesInfo.Finam
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
