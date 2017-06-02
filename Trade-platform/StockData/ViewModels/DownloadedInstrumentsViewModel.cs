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
using TradePlatform.StockData.Events;
using TradePlatform.StockData.Models;
using TradePlatform.StockData.Presenters;
using TradePlatform.StockData.Holders;

namespace TradePlatform.StockData.ViewModels
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

        private readonly IInfoPublisher _infoPublisher;
        private readonly IDownloadedInstrumentsHolder _instrumentsHolder;

        public DownloadedInstrumentsViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddPresenterToListEvent>().Subscribe(AddItemItemToList, false);
            eventAggregator.GetEvent<RemovePresenterFromListEvent>().Subscribe(RemoveItemFromList, false);
            RemoveCommand = new DelegateCommand<IDounloadInstrumentPresenter>(RemoveData, CanDoAction);
            SoftReloadCommand = new DelegateCommand<IDounloadInstrumentPresenter>(SoftReloadData, CanDoAction);
            HardReloadCommand = new DelegateCommand<IDounloadInstrumentPresenter>(HardReloadData, CanDoAction);
            OpenFolderCommand = new DelegateCommand<IDounloadInstrumentPresenter>(OpenFolderWithData , CanDoAction);
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
        }

        private void AddItemItemToList(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            if (instrument != null)
            {
                InstrumentsInfo.Add(instrument);
                _instrumentsHolder.Put(instrument.Instrument());

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

        private void OpenFolderWithData(IDounloadInstrumentPresenter presenter)
        {
            presenter.ShowDataInFolder();
        }

        private void RemoveData(IDounloadInstrumentPresenter presenter)
        {
            presenter.DeleteData();
        }

        private void SoftReloadData(IDounloadInstrumentPresenter presenter)
        {
            if (HasNoActiveDownloadingProcess())
            {
                presenter.SoftReloadData();
            }
        }

        private void HardReloadData(IDounloadInstrumentPresenter presenter)
        {
            if (HasNoActiveDownloadingProcess())
            {
                presenter.HardReloadData();
            }
        }

        private void WindowLoaded()
        {
            var updateHistory = new Task<ObservableCollection<IDounloadInstrumentPresenter>>(() =>
            {
                var instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
                return  new ObservableCollection<IDounloadInstrumentPresenter>(instrumentsHolder.GetAll()
                        .Select(i =>
                        {
                            var presenter = ContainerBuilder.Container
                            .Resolve<IDounloadInstrumentPresenter>(new DependencyOverride<Instrument>(i));
                            presenter.CheckData();
                            return presenter;
                        })
                        .ToList());
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

        private bool CanDoAction(IDounloadInstrumentPresenter presenter)
        {
            return presenter != null;
        }
    }
}
