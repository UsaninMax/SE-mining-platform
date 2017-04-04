using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.ViewModels
{
    public class DownloadedInstrumentsViewModel : BindableBase, IDownloadedInstrumentsViewModel
    {

        public DownloadedInstrumentsViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<PubSubEvent<IDounloadInstrumentPresenter>>().Subscribe(AddItemItemToList, false);
            this.RemoveItem = new DelegateCommand<IDounloadInstrumentPresenter> (RemoveItemFromList, CanRemoveItemFromList);
        }

        private readonly ObservableCollection<IDounloadInstrumentPresenter> _dounloadedInstruments = new ObservableCollection<IDounloadInstrumentPresenter>();

        public ObservableCollection<IDounloadInstrumentPresenter> InstrumentsInfo => _dounloadedInstruments;

        public ICommand RemoveItem { get; private set; }

    
        private void AddItemItemToList(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            if (instrument != null)
            {
                instrument.StartDownload();
                InstrumentsInfo.Add(instrument);
            }
        }

        private void RemoveItemFromList(object param)
        {
            var instrument  = param as IDounloadInstrumentPresenter;
            InstrumentsInfo.Remove(instrument);
        }

        private bool CanRemoveItemFromList(object param)
        {
            return param != null;
        }
    }
}
