using System.Collections.ObjectModel;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.DataSet.Events;
using TradePlatform.DataSet.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class ShowDataSetElementViewModel : BindableBase, IShowDataSetElementViewModel
    {
        public string UniqueId
        {
            get { return _uniqueId; }
            set
            {
                _uniqueId = value;
                RaisePropertyChanged();
            }
        }

        public string _uniqueId;
        private ObservableCollection<SubInstrument> _instrumentsInfo = new ObservableCollection<SubInstrument>();
        public ObservableCollection<SubInstrument> InstrumentsInfo
        {
            get { return _instrumentsInfo; }
            set
            {
                _instrumentsInfo = value;
                RaisePropertyChanged();
            }
        }

        public ShowDataSetElementViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<ShowDataSetEvent>().Subscribe(ShowStructure, false);
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