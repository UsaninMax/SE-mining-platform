using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using TradePlatform.Commons.Info;
using TradePlatform.StockData.DataServices.Trades;
using TradePlatform.StockData.Holders;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.ViewModel
{
    public class InstrumentChooseListViewModel : BindableBase, IInstrumentChooseListViewModel
    {
        public ICommand LoadedWindowCommand { get; private set; }

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


        public InstrumentChooseListViewModel()
        {
            LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
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
    }
}
