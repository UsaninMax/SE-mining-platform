using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.Presenters
{
    public interface IDataSetPresenter
    {
        void PrepareData();
        void DeleteData();
        void CheckData();
        void ShowDataInFolder();
        DataSetItem DataSet();
    }
}
