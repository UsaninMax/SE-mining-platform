
using Microsoft.Practices.Unity;
using TradePlatform.StockDataUploud.model;
using TradePlatform.StockDataUploud.view;
using TradePlatform.StockDataUploud.View;
using TradePlatform.StockDataUploud.viewModel;
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
            Container.RegisterType<ShellView>(new InjectionConstructor(typeof(ShellModel)));
            Container.RegisterType<DownloadedDataViewModel>(new InjectionConstructor(typeof(DownloadedData)));
            Container.RegisterType<DownloadedDataView>(new InjectionConstructor(typeof(DownloadedDataViewModel)));
            Container.RegisterType<DownloadNewDataViewModel>(new InjectionConstructor(typeof(DownloadNewData)));
            Container.RegisterType<DownloadNewDataView>(new InjectionConstructor(typeof(DownloadNewDataViewModel)));
            Container.RegisterType<HistoryView>();
        }
    }
}
