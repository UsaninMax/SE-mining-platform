
using Microsoft.Practices.Unity;
using Prism.Events;
using TradePlatform.Common.Securities;
using TradePlatform.Main.ViewModel;
using TradePlatform.StockDataDownload.Services;
using TradePlatform.StockDataDownload.viewModel;
using TradePlatform.view;
using TradePlatform.viewModel;

namespace TradePlatform
{
    class ContainerBuilder
    {    
        public static IUnityContainer Container { get; protected set; }

        public static void Initialize()
        {
            Container = new UnityContainer();
            InitializeShell();
        } 

        private static void InitializeShell()
        {

            Container.RegisterType<ShellView>();

            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ShellView>();
            Container.RegisterType<IShellModel, ShellModel>();

            Container.RegisterType<HistoryDataView>();
            Container.RegisterType<IHistoryInstrumentsViewModel, HistoryInstrumentsViewModel>();

            Container.RegisterType<IDownloadedInstrumentsViewModel, DownloadedInstrumentsViewModel>();
            Container.RegisterType<IDownloadNewInstrumentViewModel, DownloadNewInstrumentViewModel>();

            Container.RegisterType<IInstrumentDownloader, FinamInstrumentDownloader>();

            Container.RegisterType<IInstrumentDownloader, FinamInstrumentDownloader>();
            Container.RegisterType<ISecuritiesInfoDownloader, FinamSecuritiesInfoDownloader>();

            Container.RegisterType<SecuritiesInfo>(new ContainerControlledLifetimeManager());
        }
    }
}
