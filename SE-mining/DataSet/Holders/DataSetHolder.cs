using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using SEMining.DataSet.DataServices.Serialization;
using SEMining.DataSet.Models;
using SEMining.Commons.Loggers;

namespace SEMining.DataSet.Holders
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

        public DataSetItem Get(string uniqueId)
        {
            return _dataSet[uniqueId];
        }

        public IEnumerable<DataSetItem> GetAll()
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
                SystemLogger.Log.Error(e);
            }
        }
    }
}
