using TradePlatform.StockDataUploud.model;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using TradePlatform.StockDataUploud.Model;
using System.Windows.Input;
using Prism.Commands;

namespace TradePlatform.StockDataUploud.viewModel
{
    public class DownloadedDataViewModel : BindableBase, IDownloadedDataViewModel
    {

        public DownloadedDataViewModel()
        {
            _dounloadedInstruments.Add(new DownloadedData() { Instrument = "UUDD", From = new System.DateTime(), To = new System.DateTime(), Step = 10, Status = true});
            _dounloadedInstruments.Add(new DownloadedData() { Instrument = "AFSJD", From = new System.DateTime(), To = new System.DateTime(), Step = 0.10F, Status = false });

            this.RemoveItem = new DelegateCommand<DownloadedData> (RemoveItemFromList, CanRemoveItemFromList);
        }

        ObservableCollection<DownloadedData> _dounloadedInstruments = new ObservableCollection<DownloadedData>();

        public ObservableCollection<DownloadedData> DounloadedInstruments
        {
            get
            {
                return _dounloadedInstruments;
            }
        }

        public ICommand RemoveItem { get; private set; }

        private void RemoveItemFromList(object param)
        {
            var instrument  = param as DownloadedData;
            DounloadedInstruments.Remove(instrument);
        }

        private bool CanRemoveItemFromList(object param)
        {
            return param != null;
        }
    }
}
