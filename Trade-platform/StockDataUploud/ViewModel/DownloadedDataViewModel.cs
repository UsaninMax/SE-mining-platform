using TradePlatform.StockDataUploud.model;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using TradePlatform.StockDataUploud.Model;

namespace TradePlatform.StockDataUploud.viewModel
{
    public class DownloadedDataViewModel : BindableBase, IDownloadedDataViewModel
    {
        public DownloadedDataViewModel()
        {
            _dounloadedInstruments.Add(new DownloadedData() { Instrument = "UUDD", From = new System.DateTime(), To = new System.DateTime(), Step = 10, Status = DownloadStatus.InProgress});
            _dounloadedInstruments.Add(new DownloadedData() { Instrument = "AFSJD", From = new System.DateTime(), To = new System.DateTime(), Step = 0.10F, Status = DownloadStatus.Done });
        }

        ObservableCollection<DownloadedData> _dounloadedInstruments = new ObservableCollection<DownloadedData>();

        public ObservableCollection<DownloadedData> DounloadedInstruments
        {
            get
            {
                return _dounloadedInstruments;
            }
        }
    }
}
