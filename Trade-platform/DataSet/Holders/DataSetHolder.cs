using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.Holders
{
    public class DataSetHolder : IDataSetHolder
    {

        private readonly Dictionary<String, DataSetItem> _instrumnnets = new Dictionary<String, DataSetItem>();

        public bool CheckIfExist(String uniqueId)
        {
            return _instrumnnets.ContainsKey(uniqueId);
        }

        public void Put(DataSetItem dataSet)
        {
             _instrumnnets.Add(dataSet.Id,dataSet);
        }

        public void Remove(DataSetItem dataSet)
        {
            _instrumnnets.Remove(dataSet.Id);
        }

        public DataSetItem GetById(string id)
        {
            return _instrumnnets[id];
        }

        public IList<DataSetItem> GetAll()
        {
            return _instrumnnets.Values.ToList();
        }

        public void Store()
        {
            throw new System.NotImplementedException();
        }

        public void ReStore()
        {
            throw new System.NotImplementedException();
        }
    }
}
