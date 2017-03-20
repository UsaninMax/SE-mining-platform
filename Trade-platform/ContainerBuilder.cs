
using Microsoft.Practices.Unity;
using TradePlatform.Main.ViewModel;
using TradePlatform.StockDataUploud.model;
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
            Container.RegisterType<ShellView>();
            Container.RegisterType<IShellModel, ShellModel>();

            Container.RegisterType<HistoryDataView>();
            Container.RegisterType<IHistoryDataViewModel, HistoryDataViewModel>();

            Container.RegisterType<IDownloadedDataViewModel, DownloadedDataViewModel>(new InjectionConstructor(typeof(DownloadedData)));
            Container.RegisterType<IDownloadNewDataViewModel, DownloadNewDataViewModel>(new InjectionConstructor(typeof(DownloadNewData)));
        }
    }
}
