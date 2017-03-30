using Prism.Mvvm;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Services;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.DataServices;

namespace TradePlatform.StockDataDownload.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        private Instrument _instrument;
        private IInstrumentDownloadManager _downloadManager;

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadManager = ContainerBuilder.Container.Resolve<IInstrumentDownloadManager>();
        }

        public String Instrument
        {
            get
            {
                return _instrument.Name;
            }
        }

        public DateTime From
        {
            get
            {
                return _instrument.From;
            }
        }

        public DateTime To
        {
            get
            {
                return _instrument.To;
            }
        }

        private bool _status;

        public bool Status
        {
            get
            {
                return _status; 
            }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        public void StartDownload()
        {
            var downloadTask = new Task<bool>(() => _downloadManager.Execute(_instrument));
            downloadTask.ContinueWith((i) => Status = i.Result);
            downloadTask.Start();
        }
    }
}
