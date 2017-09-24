using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Mvvm;
using SEMining.Commons.Info;
using SEMining.Commons.Sistem;
using SEMining.StockData.DataServices.Trades;
using SEMining.StockData.Events;
using SEMining.StockData.Models;
using SE_mining_base.Info.Message;

namespace SEMining.StockData.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        private readonly Instrument _instrument;
        private readonly IInstrumentService _service;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IInfoPublisher _infoPublisher;
        private Task _download;

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _service = ContainerBuilder.Container.Resolve<IInstrumentService>();
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

            StatusMessage = Status.InProgress;
            _cancellationTokenSource = new CancellationTokenSource();
            _infoPublisher.PublishInfo(new DownloadInfo { Message = _instrument + "- start hard download" });
            _download = new Task(() => _service.Download(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            _download.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = Status.FailToDownloud;
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    StatusMessage = Status.IsReady;
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

            StatusMessage = Status.InProgress;
            _cancellationTokenSource = new CancellationTokenSource();
            _infoPublisher.PublishInfo(new DownloadInfo { Message = _instrument + "- start soft download" });
            _download = new Task(() => _service.SoftDownload(_instrument, _cancellationTokenSource.Token), _cancellationTokenSource.Token);
            _download.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = Status.FailToDownloud;
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    StatusMessage = Status.IsReady;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());

            _download.Start();

        }

        public void DeleteData()
        {
            StatusMessage = Status.Deleteing;
            var delete = new Task(() => _service.Delete(_instrument, _download, _cancellationTokenSource));
            delete.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = Status.FailToDelete;
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    var eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
                    eventAggregator.GetEvent<RemovePresenterFromListEvent>().Publish(this);
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
            StatusMessage = Status.Checking;
            var checkTask = new Task<bool>(() => _service.CheckFiles(_instrument));
            checkTask.ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    StatusMessage = Status.FailToCheck;
                    _infoPublisher.PublishException(t.Exception);
                }
                else
                {
                    if(t.Result)
                    {
                        StatusMessage = Status.IsReady;
                    } else
                    {
                        StatusMessage = Status.DataIsCorrapted;
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

