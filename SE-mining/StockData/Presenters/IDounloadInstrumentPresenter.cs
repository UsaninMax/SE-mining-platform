using SEMining.StockData.Models;

namespace SEMining.StockData.Presenters
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
        void ShowDataInFolder();
        Instrument Instrument();
    }
}
