using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.Serialization;
using TradePlatform.StockDataDownload.Events;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.ViewModels
{
    public class DownloadedInstrumentsViewModel : BindableBase, IDownloadedInstrumentsViewModel
    {
        private ObservableCollection<IDounloadInstrumentPresenter> _dounloadedInstruments = new ObservableCollection<IDounloadInstrumentPresenter>();
        public ObservableCollection<IDounloadInstrumentPresenter> InstrumentsInfo
        {
            get
            {
                return _dounloadedInstruments;
            }
            set
            {
                _dounloadedInstruments = value;
                RaisePropertyChanged();
            }
        }

        public ICommand OpenFolderCommand { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        public ICommand SoftReloadCommand { get; private set; }

        public ICommand HardReloadCommand { get; private set; }

        public ICommand LoadedWindowCommand { get; private set; }

        public ICommand ClosingWindowCommand { get; private set; }

        private readonly IInfoPublisher _infoPublisher;

        public DownloadedInstrumentsViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddToList<IDounloadInstrumentPresenter>>().Subscribe(AddItemItemToList, false);
            eventAggregator.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>().Subscribe(RemoveItemFromList, false);
            RemoveCommand = new DelegateCommand<IDounloadInstrumentPresenter>(RemoveData, CanDoActionItemFromList);
            SoftReloadCommand = new DelegateCommand<IDounloadInstrumentPresenter>(SoftReloadData, CanDoActionItemFromList);
            HardReloadCommand = new DelegateCommand<IDounloadInstrumentPresenter>(HardReloadData, CanDoActionItemFromList);
            OpenFolderCommand = new DelegateCommand<IDounloadInstrumentPresenter>(OpenFolderWithData , CanDoActionItemFromList);
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            ClosingWindowCommand = new DelegateCommand(WindowClosing);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
        }

        private void AddItemItemToList(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            if (instrument != null)
            {
                InstrumentsInfo.Add(instrument);

                if (HasNoActiveDownloadingProcess())
                {
                    instrument.SoftDownloadData();
                }
            }
        }

        private bool HasNoActiveDownloadingProcess()
        {
            bool isOk = InstrumentsInfo.All(i => !i.InDownloadingProgress());
            if (!isOk)
            {
                _infoPublisher.PublishInfo(
                    new DownloadInfo {Message = "Finam allow you to download one file at one time"});
            }
            return isOk;
        }

        private void RemoveItemFromList(IDounloadInstrumentPresenter presenter)
        {
            InstrumentsInfo.Remove(presenter);
        }

        private void OpenFolderWithData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            instrument?.ShowDataInFolder();
        }

        private void RemoveData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            instrument?.DeleteData();
        }

        private void SoftReloadData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;

            if (HasNoActiveDownloadingProcess())
            {
                instrument?.SoftReloadData();
            }
        }

        private void HardReloadData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;

            if (HasNoActiveDownloadingProcess())
            {
                instrument?.HardReloadData();
            }
        }

        private void WindowLoaded()
        {
            var updateHistory = new Task<ObservableCollection<IDounloadInstrumentPresenter>>(() =>
            {
                var serializer = ContainerBuilder.Container.Resolve<IInstrumentsStorage>();
                return new ObservableCollection<IDounloadInstrumentPresenter>(serializer
                    .ReStore()
                    .Select(i =>
                {
                    var presenter = ContainerBuilder.Container.Resolve<IDounloadInstrumentPresenter>(new DependencyOverride<Instrument>(i));
                    presenter.CheckData();
                    return presenter;
                }).ToList());
            });
            updateHistory.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    InstrumentsInfo = t.Result;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            updateHistory.Start();
        }

        private void WindowClosing()
        {
            var storeHistory = new Task(() =>
            {
                var xmlStorage = ContainerBuilder.Container.Resolve<IInstrumentsStorage>();
                xmlStorage.Store(InstrumentsInfo.Select(i =>
                {
                    i.StopDownload();
                    return i.Instrument();
                }).ToList());
            });
            storeHistory.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    _infoPublisher.PublishException(t.Exception);
                }
            });
            storeHistory.Start();
        }

        private bool CanDoActionItemFromList(object param)
        {
            return param != null;
        }
    }
}
