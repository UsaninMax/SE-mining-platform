﻿using Prism.Mvvm;
using TradePlatform.StockDataDownload.model;
using TradePlatform.StockDataDownload.Services;
using Microsoft.Practices.Unity;
using System;
using System.Threading.Tasks;

namespace TradePlatform.StockDataDownload.Presenters
{
    public class DounloadInstrumentPresenter : BindableBase, IDounloadInstrumentPresenter
    {
        Instrument _instrument;
        IDownloadInstrument _downloadInstrument;

        public DounloadInstrumentPresenter(Instrument instrument)
        {
            _instrument = instrument;
            _downloadInstrument = ContainerBuilder.Container.Resolve<IDownloadInstrument>();
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

        public float MinStep
        {
            get
            {
                return _instrument.MinStep;
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
                this._status = value;
                OnPropertyChanged();
            }
        }

        public void StartDownload()
        {
            var downloadTask = new Task<bool>(() => _downloadInstrument.download(_instrument));
            downloadTask.ContinueWith((i) => Status = i.Result);
            downloadTask.Start();
        }
    }
}
