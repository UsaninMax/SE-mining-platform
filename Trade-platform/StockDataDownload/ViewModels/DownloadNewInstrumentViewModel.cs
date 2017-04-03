using TradePlatform.StockDataDownload.model;
using Prism.Mvvm;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Commands;
using System.Windows.Input;
using TradePlatform.StockDataDownload.Presenters;
using System;
using System.Threading.Tasks;
using TradePlatform.Commons.Server;
using System.Collections.ObjectModel;
using TradePlatform.StockDataDownload.DataServices;
using TradePlatform.Common.Securities;
using TradePlatform.Commons.Securities;

namespace TradePlatform.StockDataDownload.viewModel
{
    public class DownloadNewInstrumentViewModel : BindableBase, IDownloadNewInstrumentViewModel
    {

        private bool _hideWaitSpinnerBar;
        public bool HideWaitSpinnerBar
        {
            get
            {
                return _hideWaitSpinnerBar;
            }
            set
            {
                _hideWaitSpinnerBar = value;
                RaisePropertyChanged();
            }
        }

        private String _statusMessage;
        public String StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                RaisePropertyChanged();
            }
        }

        private bool _isEnabledPanel;
        public bool IsEnabledPanel
        {
            get
            {
                return _isEnabledPanel;
            }
            set
            {
                _isEnabledPanel = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Market> _markets;
        public ObservableCollection<Market> Markets
        {
            get
            {
                return _markets;
            }
            set
            {
                _markets = value;
                RaisePropertyChanged();
            }
        }

        private Market _selectedMarket;
        public Market SelectedMarket
        {
            get
            {
                return _selectedMarket;
            }
            set
            {
                _selectedMarket = value;
                updateSecuritiesBox();
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<Security> _securities;
        public ObservableCollection<Security> Securities
        {
            get
            {
                return _securities;
            }
            set
            {
                _securities = value;
                RaisePropertyChanged();
            }
        }

        private Security _selectedSecurity;
        public Security SelectedSecurity
        {
            get
            {
                return _selectedSecurity;
            }
            set
            {
                _selectedSecurity = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _dateFrom = DateTime.Today;
        public DateTime DateFrom
        {
            get
            {
                return _dateFrom;
            }
            set
            {
                _dateFrom = value;
            }
        }

        private DateTime _dateTo = DateTime.Today;
        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                _dateTo = value;
            }
        }

        private ISecuritiesInfoUpdater _suritiesInfoUpdater;
        private SecuritiesInfo _securitiesInfo;

        public DownloadNewInstrumentViewModel()
        {
            this._securitiesInfo = ContainerBuilder.Container.Resolve<SecuritiesInfo>();
            this._suritiesInfoUpdater = ContainerBuilder.Container.Resolve<ISecuritiesInfoUpdater>();
            this.AddNew = new DelegateCommand(AddNewInstrument);
        }

        public ICommand AddNew { get; private set; }

        private void AddNewInstrument()
        {
            DounloadInstrumentPresenter presenter = new DounloadInstrumentPresenter(new Instrument() {
                Name = _selectedSecurity.Name,
                Code = _selectedSecurity.Code,
                Id = _selectedSecurity.Id,
                MarketId = _selectedSecurity.Market.Id,
                From = _dateFrom,
                To = _dateTo});
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<PubSubEvent<DounloadInstrumentPresenter>>().Publish(presenter);
        }

        public void UpdateSecuritiesInfo()
        {
            StatusMessage = OperationStatuses.SECURITIES_INFO_UPDATE_IN_PROGRESS;
            var downloadTask = new Task<bool>(() => _suritiesInfoUpdater.Update());
            downloadTask.ContinueWith((i) => UpdatePanel(i.Result));
            downloadTask.Start();
        }

        private void UpdatePanel(bool status)
        {
            HideWaitSpinnerBar = true;

            if (status == false)
            {
                StatusMessage = OperationStatuses.FAIL_TO_UPDATE_SECURITIES_INFO;
                return;
            }

            Markets = new ObservableCollection<Market>(_securitiesInfo.Markets());
            StatusMessage = OperationStatuses.SECURITIES_INFO_UPDATED;
            IsEnabledPanel = true;
        }

        private void updateSecuritiesBox()
        {
            Securities = new ObservableCollection<Security>(_securitiesInfo.SecuritiesBy(_selectedMarket));
        }
    }
}
