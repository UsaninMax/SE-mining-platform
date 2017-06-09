using System;
using System.Collections.Generic;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.DataServices
{
    public class DataSetCombiner : IDataSetCombiner
    {
        public IList<DataTick> Combine(DataSetItem item)
        {
            return  new List<DataTick>() { new DataTick()
            {
                Date = DateTime.Today, 
                Value = 333
            } };
        }
    }
}
