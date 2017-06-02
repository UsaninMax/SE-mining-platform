using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.Presenters
{
    public interface IDataSetPresenter
    {
        void PrepareData();
        void DeleteData();
        void CheckData();
        bool InPrepareDataProgress();
        void ShowDataInFolder();
        DataSetItem Instrument();
    }
}
