using TradePlatform.StockDataUploud.model;
using Prism.Mvvm;
using TradePlatform.StockDataUploud.viewModel;

namespace TradePlatform.StockDataUploud.viewModel
{
    public class DownloadNewDataViewModel : BindableBase, IDownloadNewDataViewModel
    {
        public DownloadNewDataViewModel(DownloadNewData model)
        {
            this.Model = model;
        }

        public DownloadNewData Model { get; private set; }

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
