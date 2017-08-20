using System;
using System.Collections.Generic;
using SEMining.DataSet.Models;
namespace SEMining.DataSet.Holders
{
    public interface IDataSetHolder
    {
        bool CheckIfExist(String uniqueId);
        void Put(DataSetItem dataSet);
        void Remove(DataSetItem dataSet);
        DataSetItem Get(String id);
        IEnumerable<DataSetItem> GetAll();
        void Store();
        void Restore();
    }
}
