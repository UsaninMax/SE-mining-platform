using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.Presenters
{
    public interface IDounloadInstrumentPresenter 
    {
        void SoftDownloadData();
        void DeleteData();
        void SoftReloadData();
        void HardReloadData();
        void CheckData();
        bool InDownloadingProgress();
        void StopDownload();
        Instrument Instrument();
    }
}
