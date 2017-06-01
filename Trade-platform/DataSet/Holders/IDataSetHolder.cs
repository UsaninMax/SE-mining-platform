using System;
using System.Collections.Generic;
using TradePlatform.DataSet.Models;
namespace TradePlatform.DataSet.Holders
{
    interface IDataSetHolder
    {
        bool CheckIfExist(String uniqueId);
        void Put(DataSetItem dataSet);
        void Remove(DataSetItem dataSet);
        DataSetItem GetById(String id);
        IList<DataSetItem> GetAll();
        void Store();
        void ReStore();
    }
}
