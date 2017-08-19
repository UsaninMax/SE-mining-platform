using System;
using System.Collections.Generic;
using TradePlatform.DataSet.Models;
namespace TradePlatform.DataSet.Holders
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
