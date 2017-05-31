using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.Commons.Sistem;
using TradePlatform.StockData.DataServices.Trades;
using TradePlatform.StockData.Events;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        private readonly Instrument _instrument;
        private readonly IInstrumentDownloadService _downloadService;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IInfoPublisher _infoPublisher;
        private Task _download;

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
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
            _infoPublisher.PublishInfo(new DownloadInfo { Message = _instrument + "- start hard download" });
            _download = new Task(() => _downloadService.Download(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            _download.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = TradesStatuses.FailToDownloud;
                    _infoPublisher.PublishException(t.Exception);
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
            _infoPublisher.PublishInfo(new DownloadInfo { Message = _instrument + "- start soft download" });
            _download = new Task(() => _downloadService.SoftDownload(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            _download.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = TradesStatuses.FailToDownloud;
                    _infoPublisher.PublishException(t.Exception);
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
                    _infoPublisher.PublishException(t.Exception);
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
            bool isActive = _download != null
                            && !_download.IsCompleted;
            if (isActive)
            {
                _infoPublisher.PublishInfo(new DownloadInfo {Message = this + "- currently in active download process"});
            }
            return isActive;
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
                    _infoPublisher.PublishException(t.Exception);
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

        public void ShowDataInFolder()
        {
            try
            {
                var fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
                fileManager.OpenFolder(_instrument.DataProvider + "\\" + _instrument.Path);
            }
            catch (Exception ex)
            {
                _infoPublisher.PublishException(ex);
            }
        }

        public Instrument Instrument()
        {
            return _instrument;
        }

        public override string ToString()
        {
            return $"{nameof(_instrument)}: {_instrument}," +
                   $" {nameof(Name)}: {Name}," +
                   $" {nameof(From)}: {From}," +
                   $" {nameof(To)}: {To}," +
                   $" {nameof(StatusMessage)}: {StatusMessage}";
        }
    }
}

