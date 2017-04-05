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
        private readonly Task _download;
        private readonly Task _delete;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
            _download = new Task(() => _downloadService.Download(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            _delete = new Task(() => _downloadService.Delete(_instrument, _download, _cancellationTokenSource));
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
            _download.ContinueWith(t =>
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
                if (t.IsCompleted)
                {
                    StatusMessage = TradesStatuses.Downloaded;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            _download.Start();
        }

        public void DeleteData()
        {
            StatusMessage = TradesStatuses.Deleteing;
            _delete.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    //TODO
                    Exception ex = t.Exception;
                    while (ex is AggregateException && ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
                    MessageBox.Show("Error: " + ex.Message);
                }

                if (t.IsCompleted)
                {
                    var eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>().Publish(this);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            _delete.Start();
        }
    }
}

