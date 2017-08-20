using Microsoft.Practices.ObjectBuilder2;
using SEMining.DataSet.Events;
using SEMining.DataSet.Models;

namespace SEMining.DataSet.ViewModels
{
    public class ShowDataSetElementViewModel : DataSetElementViewModel
    {
        public ShowDataSetElementViewModel()
        {
            _eventAggregator.GetEvent<ShowDataSetEvent>().Subscribe(ShowStructure);
        }

        private void ShowStructure(DataSetItem item)
        {
            UniqueId = item.Id;
            WarrantyCoverage = item.WarrantyCoverage;
            StepSize = item.StepSize;
            item.SubInstruments.ForEach(InstrumentsInfo.Add);
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventAggregator.GetEvent<ShowDataSetEvent>().Unsubscribe(ShowStructure);
        }
    }
}