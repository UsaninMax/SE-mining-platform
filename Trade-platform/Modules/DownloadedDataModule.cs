using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using TradePlatform.StockDataUploud.view;

namespace TradePlatform.Modules
{
    class DownloadedDataModule : IModule
    {
        public DownloadedDataModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }
        public void Initialize()
        {
            RegionManager.Regions["DownloadedData"].Add(Container.Resolve<DownloadedDataView>());
        }

        public IUnityContainer Container { get; private set; }
        public IRegionManager RegionManager { get; private set; }
    }
}
