using Prism.Mvvm;
using TradePlatform.StockDataDownload.model;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;
using TradePlatform.StockDataDownload.DataServices;

namespace TradePlatform.StockDataDownload.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        private Instrument _instrument;
        private IInstrumentDownloadService _downloadService;

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadService = ContainerBuilder.Container.Resolve<IInstrumentDownloadService>();
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
            var downloadTask = new Task<bool>(() => _downloadService.Execute(_instrument));
            downloadTask.ContinueWith((i) => Status = i.Result);
            downloadTask.Start();
        }
    }
}
