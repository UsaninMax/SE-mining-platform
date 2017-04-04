
using Microsoft.Practices.Unity;
using Prism.Events;
using System.Net;
using TradePlatform.Commons.Securities;
using TradePlatform.Main.ViewModels;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.DataServices.Trades.Finam;
using TradePlatform.StockDataDownload.ViewModels;
using TradePlatform.view;

namespace TradePlatform
{
    class ContainerBuilder
    {    
        public static IUnityContainer Container { get; private set; }

        public static void Initialize()
        {
            Container = new UnityContainer();
            InitializeShell();
        }

        private static void InitializeShell()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Container.RegisterType<ShellView>();

            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ShellView>();
            Container.RegisterType<IShellModel, ShellModel>();

            Container.RegisterType<IHistoryInstrumentsViewModel, HistoryInstrumentsViewModel>();

            Container.RegisterType<IDownloadedInstrumentsViewModel, DownloadedInstrumentsViewModel>();
            Container.RegisterType<IDownloadNewInstrumentViewModel, DownloadNewInstrumentViewModel>();

            Container.RegisterType<ISecuritiesInfoUpdater, FinamSecuritiesInfoUpdater>();
            Container.RegisterType<ISecuritiesInfoDownloader, FinamSecuritiesInfoDownloader>();
            Container.RegisterType<ISecuritiesInfoParser, FinamSecuritiesInfoParser>();

            Container.RegisterType<SecuritiesInfoHolder>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IInstrumentSplitter, FinamInstrumentSplitter>();


            Container.RegisterType<IInstrumentDownloadService, FinamInstrumentDownloadService>();

            Container.RegisterType<ITradesDownloader, FinamTradesDownloader>();
        }
    }
}
