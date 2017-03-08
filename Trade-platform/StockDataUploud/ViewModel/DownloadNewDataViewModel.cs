using TradePlatform.StockDataUploud.model;
using TradePlatform.common.viewModel;

namespace TradePlatform.StockDataUploud.viewModel
{
    class DownloadNewDataViewModel : BaseViewModel
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
