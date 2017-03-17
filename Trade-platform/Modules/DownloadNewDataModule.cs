﻿using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using TradePlatform.StockDataUploud.view;

namespace TradePlatform.Modules
{
    class DownloadNewDataModule : IModule
    {
        public DownloadNewDataModule(IUnityContainer container, IRegionManager regionManager)
        {
            Container = container;
            RegionManager = regionManager;
        }
        public void Initialize()
        {
            RegionManager.Regions["DownloadNewData"].Add(Container.Resolve<DownloadNewDataView>());
        }

        public IUnityContainer Container { get; private set; }
        public IRegionManager RegionManager { get; private set; }
    }
}
