using TradePlatform.StockDataUploud.model;
using Prism.Mvvm;
using TradePlatform.StockDataUploud.viewModel;

namespace TradePlatform.StockDataUploud.viewModel
{
    class DownloadedDataViewModel : BindableBase, IDownloadedDataViewModel
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
