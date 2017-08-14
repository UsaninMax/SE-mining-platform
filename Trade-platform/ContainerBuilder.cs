﻿
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Prism.Events;
using System.Net;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Exception;
using TradePlatform.Commons.Info.ViewModels;
using TradePlatform.Commons.Setting;
using TradePlatform.Commons.Sistem;
using TradePlatform.DataSet.DataServices;
using TradePlatform.DataSet.DataServices.Serialization;
using TradePlatform.DataSet.Holders;
using TradePlatform.DataSet.Models;
using TradePlatform.DataSet.Presenters;
using TradePlatform.DataSet.ViewModels;
using TradePlatform.Main.ViewModels;
using TradePlatform.Main.Views;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.DataProviding.Checks;
using TradePlatform.Sandbox.DataProviding.Transformers;
using TradePlatform.Sandbox.Presenters;
using TradePlatform.Sandbox.Providers;
using TradePlatform.Sandbox.Transactios;
using TradePlatform.Sandbox.Transactios.Models;
using TradePlatform.StockData.DataServices.SecuritiesInfo;
using TradePlatform.StockData.DataServices.SecuritiesInfo.Finam;
using TradePlatform.StockData.DataServices.Serialization;
using TradePlatform.StockData.DataServices.Trades;
using TradePlatform.StockData.DataServices.Trades.Finam;
using TradePlatform.StockData.Holders;
using TradePlatform.StockData.Models;
using TradePlatform.StockData.Presenters;
using TradePlatform.StockData.ViewModels;
using TradePlatform.Sandbox.Holders;
using System;
using TradePlatform.Charts.Data.Holders;
using TradePlatform.Charts.Data.Populating;
using TradePlatform.Charts.Data.Providers;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.Dispatching;
using TradePlatform.Charts.Vizualization.Holders;
using TradePlatform.Charts.Vizualization.ViewModels;
using TradePlatform.Sandbox.ResultProcessing;
using TradePlatform.Sandbox.ResultStoring;

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

