using SEMining.StockData.Models;


namespace SEMining.StockData.DataServices.Trades
{

    public interface ITradesDownloader
    {
        void Download(Instrument instrument);
    }
}
