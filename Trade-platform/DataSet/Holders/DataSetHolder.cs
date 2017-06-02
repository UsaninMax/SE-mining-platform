using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.DataSet.DataServices.Serialization;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.Holders
{
    public class DataSetHolder : IDataSetHolder
    {
        private readonly IDataSetStorage _dataSetStorage;
        private readonly Dictionary<String, DataSetItem> _dataSet = new Dictionary<String, DataSetItem>();

        public DataSetHolder()
        {
            _dataSetStorage = ContainerBuilder.Container.Resolve<IDataSetStorage>();
        }


        public bool CheckIfExist(String uniqueId)
        {
            return _dataSet.ContainsKey(uniqueId);
        }

        public void Put(DataSetItem dataSet)
        {
             _dataSet.Add(dataSet.Id,dataSet);
        }

        public void Remove(DataSetItem dataSet)
        {
            _dataSet.Remove(dataSet.Id);
        }

        public DataSetItem GetById(string id)
        {
            return _dataSet[id];
        }

        public IList<DataSetItem> GetAll()
        {
            return _dataSet.Values.ToList();
        }

        public void Store()
        {
            _dataSetStorage.Store(_dataSet.Values.ToList());
        }

        public void Restore()
        {
            try
            {
                _dataSetStorage
                    .ReStore()
                    .ForEach(i => _dataSet.Add(i.Id, i));
            }
            catch (Exception e)
            {
                //TODO: log
            }
        }
    }
}
