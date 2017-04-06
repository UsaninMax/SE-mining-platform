using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using TradePlatform.Commons.MessageSubscribers;
using TradePlatform.Commons.Trades;
using TradePlatform.StockDataDownload.DataServices.Serialization;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.ViewModels
{
    public class DownloadedInstrumentsViewModel : BindableBase, IDownloadedInstrumentsViewModel
    {

        public DownloadedInstrumentsViewModel()
        {
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<AddToList<IDounloadInstrumentPresenter>>().Subscribe(AddItemItemToList, false);
            eventAggregator.GetEvent<RemoveFromList<IDounloadInstrumentPresenter>>().Subscribe(RemoveItemFromList, false);
            this.RemoveCommand = new DelegateCommand<IDounloadInstrumentPresenter>(RemoveData, CanDoActionItemFromList);
            this.ReloadCommand = new DelegateCommand<IDounloadInstrumentPresenter>(ReloadData, CanDoActionItemFromList);
            this.LoadedWindowCommand = new DelegateCommand(WindowLoaded);
            this.ClosingWindowCommand = new DelegateCommand(WindowClosing);
        }

        private ObservableCollection<IDounloadInstrumentPresenter> _dounloadedInstruments = new ObservableCollection<IDounloadInstrumentPresenter>();
        public ObservableCollection<IDounloadInstrumentPresenter> InstrumentsInfo
        {
            get
            {
                return _dounloadedInstruments;
            }
            set
            {
                _dounloadedInstruments = value;
                RaisePropertyChanged();
            }
        }

        public ICommand RemoveCommand { get; private set; }

        public ICommand ReloadCommand { get; private set; }

        public ICommand LoadedWindowCommand { get; private set; }

        public ICommand ClosingWindowCommand { get; private set; }

        private void AddItemItemToList(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            if (instrument != null)
            {
                InstrumentsInfo.Add(instrument);

                if (IsExistActiveDownloading())
                {
                    //TODO: add log
                    instrument.StartDownload();
                }
            }
        }
        //Finam restriction for downloading 
        private bool IsExistActiveDownloading()
        {
            return InstrumentsInfo.All(i =>
            {
                var instrument = i as DounloadInstrumentPresenter;
                return instrument.Download == null || instrument.Download.IsCompleted;
            });
        }

        private void RemoveItemFromList(IDounloadInstrumentPresenter presenter)
        {
            InstrumentsInfo.Remove(presenter);
        }

        private void RemoveData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            instrument?.DeleteData();
        }

        private void ReloadData(object param)
        {
            var instrument = param as IDounloadInstrumentPresenter;
            if (instrument != null)
            {
                if (IsExistActiveDownloading())
                {
                    //TODO: add log
                    instrument.ReloadData();
                }
            }
        }

        private void WindowLoaded()
        {

            var updateHistory = new Task<ObservableCollection<IDounloadInstrumentPresenter>>(() =>
            {
                var serializer = ContainerBuilder.Container.Resolve<IInstrumentsSerializer>();
                return new ObservableCollection<IDounloadInstrumentPresenter>(serializer
                    .Deserialize()
                    .Select(i =>
                {
                    var presenter = new DounloadInstrumentPresenter(i);
                    presenter.Check();
                    return presenter;
                }).ToList());
            });
            updateHistory.ContinueWith(t =>
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
                else
                {
                    InstrumentsInfo = t.Result;
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            updateHistory.Start();
        }

        private void WindowClosing()
        {
          //  var serializer = ContainerBuilder.Container.Resolve<IInstrumentsSerializer>();

        }

        private bool CanDoActionItemFromList(object param)
        {
            return param != null;
        }
    }
}
