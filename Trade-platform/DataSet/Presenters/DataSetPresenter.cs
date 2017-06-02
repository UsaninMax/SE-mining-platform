using System;
using Prism.Mvvm;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.Presenters
{
    public class DataSetPresenter : BindableBase, IDataSetPresenter
    {
        private readonly DataSetItem _dataSet;
        private string _statusMessage;
        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }
        public String DataSetId => _dataSet.Id;


        public DataSetPresenter(DataSetItem dataSet)
        {
            _dataSet = dataSet;
        }

        public void PrepareData()
        {
        }

        public void DeleteData()
        {
            
        }

        public void CheckData()
        {
      
        }

        public bool InPrepareDataProgress()
        {
            throw new System.NotImplementedException();
        }

        public void ShowDataInFolder()
        {
            throw new System.NotImplementedException();
        }

        public DataSetItem DataSet()
        {
            return _dataSet;
        }
    }
}
