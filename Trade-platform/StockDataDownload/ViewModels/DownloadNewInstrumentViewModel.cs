using TradePlatform.StockDataDownload.model;
using Prism.Mvvm;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Commands;
using System.Windows.Input;
using TradePlatform.StockDataDownload.Presenters;
using System;
using TradePlatform.StockDataDownload.Services;
using System.Threading.Tasks;
using TradePlatform.Commons.Server;
using System.Collections.ObjectModel;
using TradePlatform.StockDataDownload.DataServices;

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

        private ObservableCollection<String> _securitiesClasses;
        public ObservableCollection<String> SecuritiesClasses
        {
            get
            {
                return _securitiesClasses;
            }
            set
            {
                _securitiesClasses = value;
                RaisePropertyChanged();
            }
        }

        private String _selectedSecuritiesClass;
        public String SelectedSecuritiesClass
        {
            get
            {
                return _selectedSecuritiesClass;
            }
            set
            {
                _selectedSecuritiesClass = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<String> _securitiesInstruments;
        public ObservableCollection<String> SecuritiesInstruments
        {
            get
            {
                return _securitiesInstruments;
            }
            set
            {
                _securitiesInstruments = value;
                RaisePropertyChanged();
            }
        }

        private String _selectedSecuritiesInstrument;
        public String SelectedSecuritiesInstrument
        {
            get
            {
                return _selectedSecuritiesInstrument;
            }
            set
            {
                _selectedSecuritiesInstrument = value;
                RaisePropertyChanged();
            }
        }

        private DateTime _dateFrom;
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

        private DateTime _dateTo;
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

        public DownloadNewInstrumentViewModel()
        {
            this._suritiesInfoUpdater = ContainerBuilder.Container.Resolve<ISecuritiesInfoUpdater>();
            this.AddNew = new DelegateCommand(AddNewInstrument);
        }

        public ICommand AddNew { get; private set; }

        private void AddNewInstrument()
        {
            DounloadInstrumentPresenter presenter = new DounloadInstrumentPresenter(new Instrument() { Name = _selectedSecuritiesInstrument, From = _dateFrom, To = _dateTo, MinStep = 10 });
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

            StatusMessage = OperationStatuses.SECURITIES_INFO_UPDATED;
            IsEnabledPanel = true;

        }
    }
}
