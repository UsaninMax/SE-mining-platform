
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Prism.Events;
using System.Net;
using SEMining.Commons.Info;
using SEMining.Commons.Info.Exception;
using SEMining.Commons.Info.ViewModels;
using SEMining.Commons.Setting;
using SEMining.Commons.Sistem;
using SEMining.DataSet.DataServices;
using SEMining.DataSet.DataServices.Serialization;
using SEMining.DataSet.Holders;
using SEMining.DataSet.Models;
using SEMining.DataSet.Presenters;
using SEMining.DataSet.ViewModels;
using SEMining.Main.ViewModels;
using SEMining.Main.Views;
using SEMining.Sandbox;
using SEMining.Sandbox.DataProviding;
using SEMining.Sandbox.DataProviding.Checks;
using SEMining.Sandbox.DataProviding.Transformers;
using SEMining.Sandbox.Presenters;
using SEMining.Sandbox.Providers;
using SEMining.Sandbox.Transactios;
using SEMining.Sandbox.Transactios.Models;
using SEMining.StockData.DataServices.SecuritiesInfo;
using SEMining.StockData.DataServices.SecuritiesInfo.Finam;
using SEMining.StockData.DataServices.Serialization;
using SEMining.StockData.DataServices.Trades;
using SEMining.StockData.DataServices.Trades.Finam;
using SEMining.StockData.Holders;
using SEMining.StockData.Models;
using SEMining.StockData.Presenters;
using SEMining.StockData.ViewModels;
using SEMining.Sandbox.Holders;
using System;
using SEMining.Charts.Data.Holders;
using SEMining.Charts.Data.Populating;
using SEMining.Charts.Data.Providers;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.Dispatching;
using SEMining.Charts.Vizualization.Holders;
using SEMining.Charts.Vizualization.ViewModels;
using SEMining.Sandbox.Results.Storing;

namespace SEMining
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            Container.RegisterType<ShellView>();
            Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ShellView>();
            Container.RegisterType<IShellModel, ShellModel>();

            Container.RegisterType<IFileManager, FileManager>();
            Container.RegisterType<IInfoViewModel, InfoViewModel>();
            Container.RegisterInstance(new ExceptionActualizer());
            Container.RegisterType<ISettingSerializer, XmlSettingSerializer>();
            Container.RegisterType<IInfoPublisher, InfoPublisher>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IHistoryInstrumentsViewModel, HistoryInstrumentsViewModel>();
            Container.RegisterType<IDownloadedInstrumentsViewModel, DownloadedInstrumentsViewModel>();
            Container.RegisterType<IDownloadNewInstrumentViewModel, FinamDownloadNewInstrumentViewModel>();
            Container.RegisterType<ISecuritiesInfoUpdater, FinamSecuritiesInfoUpdater>();
            Container.RegisterType<ISecuritiesInfoDownloader, FinamSecuritiesInfoDownloader>();
            Container.RegisterType<ISecuritiesInfoParser, FinamSecuritiesInfoParser>();
            Container.RegisterType<SecuritiesInfoHolder>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IInstrumentSplitter, FinamInstrumentSplitter>();
            Container.RegisterType<IInstrumentService, FinamInstrumentService>();
            Container.RegisterType<ITradesDownloader, FinamTradesDownloader>();
            Container.RegisterType<IInstrumentsStorage, XmlInstrumentStorage>();
            Container.RegisterType<IDounloadInstrumentPresenter, DounloadInstrumentPresenter>(new InjectionConstructor(typeof(Instrument)));
            Container.RegisterType<IDownloadedInstrumentsHolder, DownloadedInstrumentsHolder>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IDataSetElementViewModel, DataSetElementViewModel>("DataSet");
            Container.RegisterType<IDataSetElementViewModel, ShowDataSetElementViewModel>("ShowDataSet");
            Container.RegisterType<IDataSetElementViewModel, CopyDataSetElementViewModel>("CopyDataSet");
            Container.RegisterType<IDataSetListViewModel, DataSetListViewModel>();
            Container.RegisterType<IInstrumentChooseListViewModel, InstrumentChooseListViewModel>();
            Container.RegisterType<IDataSetHolder, DataSetHolder>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDataSetPresenter, DataSetPresenter>(new InjectionConstructor(typeof(DataSetItem)));
            Container.RegisterType<IDataSetStorage, XmlDataSetStorage>();
            Container.RegisterType<IDataSetService, DataSetService>();
            Container.RegisterType<IDataTickProvider, DataTickProvider>();
            Container.RegisterType<IDataTickStorage, XmlDataTickStorage>();
            Container.RegisterType<IDataTickParser, FinamDataTickParser>();

            Container.RegisterType<ISandboxPresenter, SandboxPresenter>(new InjectionConstructor(typeof(ISandbox) , typeof(string)));
            Container.RegisterType<ISandboxProvider, SandboxProvider>();
            Container.RegisterType<ISandboxDataHolder, SandboxDataHolder>(new ContainerControlledLifetimeManager());


            Container.RegisterType<IPredicateChecker, SlicePredicateChecker>();
            Container.RegisterType<ISandboxDataProvider, SandboxDataProvider>();
            Container.RegisterType<ITransformer, DataTransformer>();
            Container.RegisterType<IIndicatorBuilder, IndicatorBuilder>();

            Container.RegisterType<ITransactionsContext, TransactionsContext>(new InjectionConstructor(typeof(IDictionary<string, BrokerCost>)));
            Container.RegisterType<ITransactionHolder, TransactionHolder>(new InjectionConstructor(typeof(IDictionary<string, BrokerCost>)));
            Container.RegisterType<IBalance, Balance>();
            Container.RegisterType<ITransactionBuilder, TransactionBuilder>();
            Container.RegisterType<IWorkingPeriodHolder, WorkingPeriodHolder>();

            Container.RegisterType<IChartViewModel, DateChartViewModel>("DateChartViewModel", new InjectionConstructor(typeof(TimeSpan)));
            Container.RegisterType<IChartViewModel, IndexChartViewModel>("IndexChartViewModel");
            Container.RegisterType<IChartsConfigurationDispatcher, ChartsConfigurationDispatcher>();
            Container.RegisterType<IChartsHolder, ChartsHolder>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IChartsPopulator, ChartsPopulator>(new ContainerControlledLifetimeManager(),new InjectionConstructor(typeof(IEnumerable<PanelViewPredicate>)));
            Container.RegisterType<IChartDataProvider, ChartDataProvider>();
            Container.RegisterType<IChartsBuilder, ChartsBuilder>();
            Container.RegisterType<IChartProxy, ChartProxy>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IChartPredicatesHolder, ChartPredicatesHolder>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ICustomDataHolder, CustomDataHolder>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IResultStoring, StoringToFile>(new ContainerControlledLifetimeManager());
        }
    }
}

