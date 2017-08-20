using Microsoft.Practices.ObjectBuilder2;
using SEMining.DataSet.Events;
using SEMining.DataSet.Models;

namespace SEMining.DataSet.ViewModels
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
