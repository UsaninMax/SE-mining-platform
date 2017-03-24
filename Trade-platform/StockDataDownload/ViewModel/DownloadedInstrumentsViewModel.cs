using Prism.Mvvm;
using System.Collections.ObjectModel;
using Microsoft.Practices.Unity;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.viewModel
{
    public class DownloadedInstrumentsViewModel : BindableBase, IDownloadedInstrumentsViewModel
    {

        public DownloadedInstrumentsViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<PubSubEvent<DounloadInstrumentPresenter>>().Subscribe(AddItemItemToList, false);
            this.RemoveItem = new DelegateCommand<DounloadInstrumentPresenter> (RemoveItemFromList, CanRemoveItemFromList);
        }

        ObservableCollection<DounloadInstrumentPresenter> _dounloadedInstruments = new ObservableCollection<DounloadInstrumentPresenter>();

        public ObservableCollection<DounloadInstrumentPresenter> InstrumentsInfo
        {
            get
            {
                return _dounloadedInstruments;
            }
        }

        public ICommand RemoveItem { get; private set; }

    
        private void AddItemItemToList(object param)
        {
            var instrument = param as DounloadInstrumentPresenter;
            instrument.StartDownload();
            InstrumentsInfo.Add(instrument);
        }

        private void RemoveItemFromList(object param)
        {
            var instrument  = param as DounloadInstrumentPresenter;
            InstrumentsInfo.Remove(instrument);
        }

        private bool CanRemoveItemFromList(object param)
        {
            return param != null;
        }
    }
}
