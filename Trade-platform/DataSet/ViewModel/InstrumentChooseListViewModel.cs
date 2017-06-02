using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.BaseModels;
using TradePlatform.Commons.Info;
using TradePlatform.DataSet.Events;
using TradePlatform.StockData.DataServices.Trades;
using TradePlatform.StockData.Holders;
using TradePlatform.StockData.Models;
using DelegateCommand = Prism.Commands.DelegateCommand;

namespace TradePlatform.DataSet.ViewModel
{
    public class InstrumentChooseListViewModel : BindableBase, IInstrumentChooseListViewModel, IClosableWindow
    {
        public event EventHandler CloseWindowNotification;
        public ICommand LoadedWindowCommand { get; private set; }
        public ICommand AddSelectedItemsCommand { get; private set; }

        private ObservableCollection<Instrument> _instrumentsInfo = new ObservableCollection<Instrument>();
        public ObservableCollection<Instrument> InstrumentsInfo
        {
            get
            {
                return _instrumentsInfo;
            }
            set
            {
                _instrumentsInfo = value;
                RaisePropertyChanged();
            }
        }

        private readonly IInfoPublisher _infoPublisher;
        private readonly IEventAggregator _eventAggregator;
        public InstrumentChooseListViewModel()
        {
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            AddSelectedItemsCommand = new DelegateCommand<IList>(AddSelectedItems);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
        }

        private void AddSelectedItems(IList parameter)
        {
            var selectedInstruments = parameter.Cast<Instrument>().ToList();
            _eventAggregator.GetEvent<AddInstrumentToDatatSetEvent>().Publish(selectedInstruments);
            CloseWindowNotify();
        }

        private void WindowLoaded()
        {
            var updateHistory = new Task<ObservableCollection<Instrument>>(() =>
            {
                var instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
                var downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
                return new ObservableCollection<Instrument>(instrumentsHolder.GetAll().Where(i =>downloadService.CheckFiles(i)));
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

        private void CloseWindowNotify()
        {
            CloseWindowNotification?.Invoke(this, EventArgs.Empty);
        }
    }
}
