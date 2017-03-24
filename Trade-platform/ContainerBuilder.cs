
using Microsoft.Practices.Unity;
using Prism.Events;
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

        public static void initialize()
        {
            Container = new UnityContainer();
            initializeShell();
        } 

        private static void initializeShell()
        {

            Container.RegisterType<ShellView>();

            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ShellView>();
            Container.RegisterType<IShellModel, ShellModel>();

            Container.RegisterType<HistoryDataView>();
            Container.RegisterType<IHistoryInstrumentsViewModel, HistoryInstrumentsViewModel>();

            Container.RegisterType<IDownloadedInstrumentsViewModel, DownloadedInstrumentsViewModel>();
            Container.RegisterType<IDownloadNewInstrumentViewModel, DownloadNewInstrumentViewModel>();

            Container.RegisterType<IDownloadInstrument, DownloadFinamInstrument>();
        }
    }
}
