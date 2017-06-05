using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class ShowDataSetElementViewModel : DataSetElementViewModel
    {
        public ShowDataSetElementViewModel()
        {
            _eventAggregator.GetEvent<ShowDataSetEvent>().Subscribe(ShowStructure, false);
        }

        private void ShowStructure(DataSetItem item)
        {
            if (_uniqueId != null)
            {
                return;
            }

            UniqueId = item.Id;
            item.SubInstruments.ForEach(InstrumentsInfo.Add);
        }
    }
}