using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SEMining.Commons.Info;
using SEMining.Commons.Info.Model.Message;
using SEMining.StockData.DataServices.SecuritiesInfo;
using SEMining.StockData.Events;
using SEMining.StockData.Holders;
using SEMining.StockData.Models;

namespace SEMining.StockData.ViewModels
{
    public class FinamDownloadNewInstrumentViewModel : BindableBase, IDownloadNewInstrumentViewModel
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
                UpdateSecuritiesBox();
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

        private readonly ISecuritiesInfoUpdater _suritiesInfoUpdater;
        private readonly SecuritiesInfoHolder _securitiesInfo;
        private readonly IInfoPublisher _infoPublisher;

        public FinamDownloadNewInstrumentViewModel()
        {
            _securitiesInfo = ContainerBuilder.Container.Resolve<SecuritiesInfoHolder>();
            _suritiesInfoUpdater = ContainerBuilder.Container.Resolve<ISecuritiesInfoUpdater>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            AddNewCommand = new DelegateCommand(AddNewInstrument);
        }

        public ICommand AddNewCommand { get; private set; }

        public void AddNewInstrument()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddInstrumentToListEvent>().Publish(new Instrument.Builder()
                .WithFrom(_dateFrom)
                .WithTo(_dateTo)
                .WithCode(_selectedSecurity?.Code)
                .WithMarketId(_selectedSecurity?.Market.Id)
                .WithName(_selectedSecurity?.Name)
                .WithId(_selectedSecurity?.Id)
                .WithDataProvider("FINAM")
                .Build());
        }

        public void UpdateSecuritiesInfo()
        {
            StatusMessage = SecuritiesInfoStatuses.SecuritiesInfoUpdateInProgress;
            var downloadTask = new Task(() => _suritiesInfoUpdater.Update());
            downloadTask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = SecuritiesInfoStatuses.FailToUpdateSecuritiesInfo;
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    Markets = new ObservableCollection<Market>(_securitiesInfo.Markets());
                    StatusMessage = SecuritiesInfoStatuses.SecuritiesInfoUpdated;
                    IsEnabledPanel = true;
                    _infoPublisher.PublishInfo(new DownloadInfo { Message = "Securities were updated" });
                }

                HideWaitSpinnerBar = true;

            });
            downloadTask.Start();
        }

        private void UpdateSecuritiesBox()
        {
            Securities = new ObservableCollection<Security>(_securitiesInfo.SecuritiesBy(_selectedMarket));
        }
    }
}
