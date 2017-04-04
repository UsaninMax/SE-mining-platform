using Prism.Mvvm;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.DataServices.Trades;
using System.Windows;
using TradePlatform.Commons.Trades;

namespace TradePlatform.StockDataDownload.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        private readonly Instrument _instrument;
        private readonly IInstrumentDownloadService _downloadService;

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
            var downloadTask = new Task(() => _downloadService.Execute(_instrument));
            downloadTask.ContinueWith((t) =>
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
                else if (t.IsCanceled)
                {

                }
                else
                {
                    StatusMessage = TradesStatuses.Downloaded;
                }
            });
            downloadTask.Start();
        }
    }
}

