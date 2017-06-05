using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class CopyDataSetElementViewModel : DataSetElementViewModel
    {

        public CopyDataSetElementViewModel()
        {
            _eventAggregator.GetEvent<CopyDataSetEvent>().Subscribe(CopyDataSet, false);
        }

        private void CopyDataSet(DataSetItem item)
        {
            item.SubInstruments.ForEach(InstrumentsInfo.Add);
        }
    }
}
