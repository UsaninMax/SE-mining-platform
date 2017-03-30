namespace TradePlatform.StockDataDownload.Services
{
    class FinamSecuritiesInfoDownloader : ISecuritiesInfoDownloader
    {
        private string _url = "http://www.finam.ru/cache/icharts/icharts.js";
        public string Download() 
        {
            var wb = new System.Net.WebClient();
            return wb.DownloadString(_url);
        }
    }
}
