using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class CopyDataSetElementViewModel : DataSetElementViewModel
    {

        public CopyDataSetElementViewModel()
        {
            _eventAggregator.GetEvent<CopyDataSetEvent>().Subscribe(CopyDataSet);
        }

        private void CopyDataSet(DataSetItem item)
        {
            WarrantyCoverage = item.WarrantyCoverage;
            StepSize = item.StepSize;
            item.SubInstruments.ForEach(InstrumentsInfo.Add);
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventAggregator.GetEvent<CopyDataSetEvent>().Unsubscribe(CopyDataSet);
        }
    }
}
