﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.MessageSubscribers;
using TradePlatform.Commons.Securities;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.ViewModels
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

        public DownloadNewInstrumentViewModel()
        {
            this._securitiesInfo = ContainerBuilder.Container.Resolve<SecuritiesInfoHolder>();
            this._suritiesInfoUpdater = ContainerBuilder.Container.Resolve<ISecuritiesInfoUpdater>();
            this.AddNew = new DelegateCommand(AddNewInstrument);
        }

        public ICommand AddNew { get; private set; }

        private void AddNewInstrument()
        {
            IDounloadInstrumentPresenter presenter = new DounloadInstrumentPresenter(new Instrument.Builder()
                    .WithFrom(_dateFrom)
                    .WithTo(_dateTo)
                    .WithCode(_selectedSecurity.Code)
                    .WithMarketId(_selectedSecurity.Market.Id)
                    .WithName(_selectedSecurity.Name)
                    .WithId(_selectedSecurity.Id)
                    .Build());
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddToList<IDounloadInstrumentPresenter>>().Publish(presenter);
        }

        public void UpdateSecuritiesInfo()
        {
            StatusMessage = SecuritiesInfoStatuses.SecuritiesInfoUpdateInProgress;
            var downloadTask = new Task(() => _suritiesInfoUpdater.Update());
            downloadTask.ContinueWith((t) =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = SecuritiesInfoStatuses.FailToUpdateSecuritiesInfo;

                    //TODO
                    Exception ex = t.Exception;
                    while (ex is AggregateException && ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show("Error: " + ex.Message);
                }
                else
                {
                    Markets = new ObservableCollection<Market>(_securitiesInfo.Markets());
                    StatusMessage = SecuritiesInfoStatuses.SecuritiesInfoUpdated;
                }

                HideWaitSpinnerBar = true;
                IsEnabledPanel = true;
            });
            downloadTask.Start();
        }

        private void UpdateSecuritiesBox()
        {
            Securities = new ObservableCollection<Security>(_securitiesInfo.SecuritiesBy(_selectedMarket));
        }
    }
}
