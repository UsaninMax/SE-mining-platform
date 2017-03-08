using System.Windows.Input;
using TradePlatform.StockDataUploud.model;
using TradePlatform.common.viewModel;
using TradePlatform.StockDataUploud.viewModel;

namespace TradePlatform.viewModel
{
    class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
            // Hook up Commands to associated methods
            this.LoadDownloadedDataCommand = new DelegateCommand(o => this.LoadDownloadedDataPage());
            this.LoadDownloadNewDataCommand = new DelegateCommand(o => this.LoadDownloadNewDataPage());
        }

        public ICommand LoadDownloadedDataCommand { get; private set; }
        public ICommand LoadDownloadNewDataCommand { get; private set; }

        // ViewModel that is currently bound to the ContentControl
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                _currentViewModel = value;
                this.OnPropertyChanged();
            }
        }
        // TODO: locator 
        private void LoadDownloadedDataPage()
        {
            CurrentViewModel = new DownloadedDataViewModel(
                new DownloadedData() { PageTitle = "This is the downloaded data Page." });
        }

        private void LoadDownloadNewDataPage()
        {
            CurrentViewModel = new DownloadNewDataViewModel(
                new DownloadNewData() { PageTitle = "This is the download NEW data Page." });
        }
    }
}
