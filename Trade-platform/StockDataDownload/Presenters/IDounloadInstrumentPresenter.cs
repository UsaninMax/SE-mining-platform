namespace TradePlatform.StockDataDownload.Presenters
{
    public interface IDounloadInstrumentPresenter 
    {
        void StartDownload();
        void DeleteData();
        void ReloadData();
        bool Check();
    }
}
