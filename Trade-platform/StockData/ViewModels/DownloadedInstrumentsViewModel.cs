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

        private IDounloadInstrumentPresenter _selectedPresenter;
        public IDounloadInstrumentPresenter SelectedPresenter
        {
            get { return _selectedPresenter; }
            set
            {
                _selectedPresenter = value;
                RaisePropertyChanged();
                UpdateVisibilityOfContextMenu();
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
            RemoveCommand = new DelegateCommand(RemoveData, CanDoAction);
            SoftReloadCommand = new DelegateCommand(SoftReloadData, CanDoAction);
            HardReloadCommand = new DelegateCommand(HardReloadData, CanDoAction);
            OpenFolderCommand = new DelegateCommand(OpenFolderWithData, CanDoAction);
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
        }

        private void AddItemItemToList(Instrument instrument)
        {
            IDounloadInstrumentPresenter presenter = ContainerBuilder.Container.Resolve<IDounloadInstrumentPresenter>(
                new DependencyOverride<Instrument>(instrument));
            InstrumentsInfo.Add(presenter);
            _instrumentsHolder.Put(presenter.Instrument());

            if (HasNoActiveDownloadingProcess())
            {
                presenter.SoftDownloadData();
            }
        }

        private bool HasNoActiveDownloadingProcess()
        {
            bool isOk = InstrumentsInfo.All(i => !i.InDownloadingProgress());
            if (!isOk)
            {
                _infoPublisher.PublishInfo(
                    new DownloadInfo { Message = "Finam allow you to download one file at one time" });
            }
            return isOk;
        }

        private void RemoveItemFromList(IDounloadInstrumentPresenter presenter)
        {
            InstrumentsInfo.Remove(presenter);
        }

        private void OpenFolderWithData()
        {
            _selectedPresenter.ShowDataInFolder();
        }

        private void RemoveData()
        {
            _selectedPresenter.DeleteData();
        }

        private void SoftReloadData()
        {
            if (HasNoActiveDownloadingProcess())
            {
                _selectedPresenter.SoftReloadData();
            }
        }

        private void HardReloadData()
        {
            if (HasNoActiveDownloadingProcess())
            {
                _selectedPresenter.HardReloadData();
            }
        }

        private void WindowLoaded()
        {
            var updateHistory = new Task<ObservableCollection<IDounloadInstrumentPresenter>>(() =>
            {
                var instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
                return new ObservableCollection<IDounloadInstrumentPresenter>(instrumentsHolder.GetAll()
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

        private void UpdateVisibilityOfContextMenu()
        {
            (RemoveCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (SoftReloadCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (HardReloadCommand as DelegateCommand)?.RaiseCanExecuteChanged();
            (OpenFolderCommand as DelegateCommand)?.RaiseCanExecuteChanged();
        }

        private bool CanDoAction()
        {
            return _selectedPresenter != null;
        }
    }
}
