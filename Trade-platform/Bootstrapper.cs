﻿using System.Windows;
using Prism.Unity;
using Microsoft.Practices.Unity;
using TradePlatform.Main.Views;
using TradePlatform.StockData.Holders;
using System.Threading.Tasks;
using TradePlatform.DataSet.Holders;
using TradePlatform.Commons.Loggers;

namespace TradePlatform
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override IUnityContainer CreateContainer()
        {
            return ContainerBuilder.Container;
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
            InitializeHolders();
            SystemLogger.InitLogger();
        }

        private void InitializeHolders()
        {
            Task.Factory.StartNew(() => {
                var instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
                instrumentsHolder.Restore();
            });

            Task.Factory.StartNew(() => {
                var dataSetsHolder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
                dataSetsHolder.Restore();
            });
        }
    }
}
