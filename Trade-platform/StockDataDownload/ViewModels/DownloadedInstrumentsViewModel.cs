using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.MessageSubscribers;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.ViewModels
{
    public class DownloadedInstrumentsViewModel : BindableBase, IDownloadedInstrumentsViewModel
    {

        public DownloadedInstrumentsViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddToList<IDounloadInstrumentPresenter>>().Subscribe(AddItemItemToList, false);
            eventAggregator.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>().Subscribe(RemoveItemFromList, false);
            this.RemoveItem = new DelegateCommand<IDounloadInstrumentPresenter> (RemoveData, CanDoActionItemFromList);
            this.ReloadItem = new DelegateCommand<IDounloadInstrumentPresenter>(ReloadData, CanDoActionItemFromList);
        }

        private readonly ObservableCollection<IDounloadInstrumentPresenter> _dounloadedInstruments = new ObservableCollection<IDounloadInstrumentPresenter>();

        public ObservableCollection<IDounloadInstrumentPresenter> InstrumentsInfo => _dounloadedInstruments;

        public ICommand RemoveItem { get; private set; }

        public  ICommand ReloadItem { get; private set; }

        private void AddItemItemToList(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            if (instrument != null)
            {
                InstrumentsInfo.Add(instrument);
                instrument.StartDownload();
            }
        }

        private void RemoveItemFromList(IDounloadInstrumentPresenter presenter)
        {
            InstrumentsInfo.Remove(presenter);
        }

        private void RemoveData(object param)
        {
            var instrument  = param as IDounloadInstrumentPresenter;
            instrument?.DeleteData();
        }

        private void ReloadData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            instrument?.ReloadData();
        }

        private bool CanDoActionItemFromList(object param)
        {
            return param != null;
        }
    }
}
