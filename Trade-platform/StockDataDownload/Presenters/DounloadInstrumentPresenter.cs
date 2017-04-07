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

        private Task _download;

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
        }

        public string Name => _instrument.Name;

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

        public void HardDownloadData()
        {
            if (IsActiveDowloadTask())
            {
                return;
            }

            StatusMessage = TradesStatuses.InProgress;
            _cancellationTokenSource = new CancellationTokenSource();
            _download = new Task(() => _downloadService.Download(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
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
                else
                {
                    StatusMessage = TradesStatuses.IsReady;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            _download.Start();
        }

        public void SoftDownloadData()
        {
            if (IsActiveDowloadTask())
            {
                return;
            }

            StatusMessage = TradesStatuses.InProgress;
            _cancellationTokenSource = new CancellationTokenSource();
            _download = new Task(() => _downloadService.SoftDownload(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
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
                else
                {
                    StatusMessage = TradesStatuses.IsReady;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            _download.Start();

        }

        public void DeleteData()
        {
            StatusMessage = TradesStatuses.Deleteing;
            var delete = new Task(() => _downloadService.Delete(_instrument, _download, _cancellationTokenSource));
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

        private bool IsActiveDowloadTask()
        {
            return _download != null
                   && !_download.IsCompleted;
        }

        public void HardReloadData()
        {
            HardDownloadData();
        }

        public void SoftReloadData()
        {
            SoftDownloadData();
        }

        public void CheckData()
        {
            StatusMessage = TradesStatuses.Checking;
            var checkTask = new Task<bool>(() => _downloadService.CheckFiles(_instrument));
            checkTask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = TradesStatuses.FailToCheck;

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
                    if(t.Result)
                    {
                        StatusMessage = TradesStatuses.IsReady;
                    } else
                    {
                        StatusMessage = TradesStatuses.DataIsCorrapted;
                    }
                }
            });

            checkTask.Start();
        }

        public bool InDownloadingProgress()
        {
            return _download != null && !_download.IsCompleted;
        }

        public void StopDownload()
        {
            _cancellationTokenSource?.Cancel();
        }

        public Instrument Instrument()
        {
            return _instrument;
        }
    }
}

