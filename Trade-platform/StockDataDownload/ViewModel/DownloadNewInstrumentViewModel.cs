using TradePlatform.StockDataDownload.model;
using Prism.Mvvm;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Commands;
using System.Windows.Input;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.StockDataDownload.viewModel
{
    public class DownloadNewInstrumentViewModel : BindableBase, IDownloadNewInstrumentViewModel
    {
        public DownloadNewInstrumentViewModel()
        {
            this.AddNew = new DelegateCommand(AddNewInstrument);
        }

        public ICommand AddNew { get; private set; }

        private void AddNewInstrument()
        {
            DounloadInstrumentPresenter presenter = new DounloadInstrumentPresenter(new Instrument() { Name = "UUDD", From = new System.DateTime(), To = new System.DateTime(), MinStep = 10 });
            IEventAggregator eventAggregator = ContainerBuilder.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<PubSubEvent<DounloadInstrumentPresenter>>().Publish(presenter);
        }
    }
}
