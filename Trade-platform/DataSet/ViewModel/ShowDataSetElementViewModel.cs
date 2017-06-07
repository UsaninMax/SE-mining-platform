using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.ViewModel
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
            item.SubInstruments.ForEach(InstrumentsInfo.Add);
        }

        public override void Dispose()
        {
            base.Dispose();
            _eventAggregator.GetEvent<ShowDataSetEvent>().Unsubscribe(ShowStructure);
        }
    }
}