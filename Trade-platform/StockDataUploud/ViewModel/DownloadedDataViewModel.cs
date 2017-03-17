using TradePlatform.StockDataUploud.model;
using Prism.Mvvm;
using TradePlatform.Common.ViewModel;

namespace TradePlatform.StockDataUploud.viewModel
{
    class DownloadedDataViewModel : BindableBase, IViewModel
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
