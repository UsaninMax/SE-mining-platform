
using Microsoft.Practices.Unity;
using Prism.Events;
using System.Net;
using TradePlatform.Commons.Holders;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Exception;
using TradePlatform.Commons.Info.ViewModels;
using TradePlatform.Commons.Securities;
using TradePlatform.Commons.Setting;
using TradePlatform.Commons.Sistem;
using TradePlatform.Commons.Trades;
using TradePlatform.Main.ViewModels;
using TradePlatform.Main.Views;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo;
using TradePlatform.StockDataDownload.DataServices.SecuritiesInfo.Finam;
using TradePlatform.StockDataDownload.DataServices.Serialization;
using TradePlatform.StockDataDownload.DataServices.Trades;
using TradePlatform.StockDataDownload.DataServices.Trades.Finam;
using TradePlatform.StockDataDownload.Presenters;
using TradePlatform.StockDataDownload.ViewModels;

namespace TradePlatform
{
    public static class ContainerBuilder
    {

        private static IUnityContainer _container;

        public static IUnityContainer Container
        {
            set { _container = value; }
            get { return _container ?? (_container = new UnityContainer()); }

        }

        public static void Initialize()
        {
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
            Container.RegisterType<IDownloadNewInstrumentViewModel, FinamDownloadNewInstrumentViewModel>();

            Container.RegisterType<ISecuritiesInfoUpdater, FinamSecuritiesInfoUpdater>();
            Container.RegisterType<ISecuritiesInfoDownloader, FinamSecuritiesInfoDownloader>();
            Container.RegisterType<ISecuritiesInfoParser, FinamSecuritiesInfoParser>();

            Container.RegisterType<SecuritiesInfoHolder>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IInstrumentSplitter, FinamInstrumentSplitter>();


            Container.RegisterType<IInstrumentDownloadService, FinamInstrumentDownloadService>();

            Container.RegisterType<ITradesDownloader, FinamTradesDownloader>();

            Container.RegisterType<IInstrumentsStorage, XmlInstrumentStorage>();

            Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));

            Container.RegisterType<IFileManager, FileManager>();

            Container.RegisterType<IInfoViewModel, InfoViewModel>();

            Container.RegisterInstance(new ExceptionActualizer());

            Container.RegisterType<ISettingSerializer, XMLSettingSerializer>();

            Container.RegisterType<IInfoPublisher, InfoPublisher>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDownloadedInstrumentsHolder, DownloadedInstrumentsHolder>(new ContainerControlledLifetimeManager());
        }
    }
}

