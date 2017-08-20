using SEMining.DataSet.Models;

namespace SEMining.DataSet.Presenters
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
