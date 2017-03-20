using TradePlatform.StockDataUploud.model;
using Prism.Mvvm;

namespace TradePlatform.StockDataUploud.viewModel
{
    public class DownloadedDataViewModel : BindableBase, IDownloadedDataViewModel
    {
        public DownloadedDataViewModel(DownloadedData model)
        {
            this.Model = model;
        }

        public DownloadedData Model { get; private set; }

        public string PageTitle
        {
            get
            {
                return this.Model.PageTitle;
            }
            set
            {
                this.Model.PageTitle = value;
                this.OnPropertyChanged();
            }
        }
    }
}
