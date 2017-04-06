using Prism.Mvvm;
using Microsoft.Practices.Unity;
using System;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.DataServices.Trades;
using System.Windows;
using Prism.Events;
using TradePlatform.Commons.MessageSubscribers;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        private readonly Instrument _instrument;
        private readonly IInstrumentDownloadService _downloadService;
        private CancellationTokenSource _cancellationTokenSource;

        public Task Download
        {
            get;
            private set;
        }

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
        }

        public string Instrument => _instrument.Name;

        public DateTime From => _instrument.From;

        public DateTime To => _instrument.To;

        private string _statusMessage;

        public string StatusMessage
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

        public void StartDownload()
        {
            StatusMessage = TradesStatuses.InProgress;
            _cancellationTokenSource = new CancellationTokenSource();
            Download = new Task(() => _downloadService.Download(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            Download.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = TradesStatuses.FailToDownloud;

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
                    StatusMessage = TradesStatuses.Downloaded;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Download.Start();
        }

        public void DeleteData()
        {
            StatusMessage = TradesStatuses.Deleteing;
            var delete = new Task(() => _downloadService.Delete(_instrument, Download, _cancellationTokenSource));
            delete.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = TradesStatuses.FailToDelete;
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
                    var eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>().Publish(this);

                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            delete.Start();
        }

        public void ReloadData()
        {
            if (Download != null
                && !Download.IsCompleted)
            {
                return;
            }

            StartDownload();
        }
    }
}

