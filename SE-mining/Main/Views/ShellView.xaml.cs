using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Unity;
using SEMining.DataSet.Holders;
using SEMining.Main.ViewModels;
using SEMining.StockData.Holders;

namespace SEMining.Main.Views
{
    public partial class ShellView : Window
    {
        private readonly Dispatcher _dispatcher;

        public ShellView()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;
            this.DataContext = ContainerBuilder.Container.Resolve<IShellModel>();
            this.InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Task.Factory.StartNew(() =>
                {
                    ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>().Store();
                    ContainerBuilder.Container.Resolve<IDataSetHolder>().Store();
                })
                .ContinueWith(res =>
                {
                    _dispatcher.BeginInvoke((Action)(() =>
                    {
                        Application.Current.Shutdown();
                    }));
                })
                .Wait();
        }
    }
}
