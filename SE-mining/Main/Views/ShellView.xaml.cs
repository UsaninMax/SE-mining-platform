using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using SEMining.DataSet.Holders;
using SEMining.Main.ViewModels;
using SEMining.StockData.Holders;

namespace SEMining.Main.Views
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            this.DataContext = ContainerBuilder.Container.Resolve<IShellModel>();
            this.InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                var instrumentsHolder = ContainerBuilder.Container.Resolve<IDownloadedInstrumentsHolder>();
                instrumentsHolder.Store();
            });

            Task.Factory.StartNew(() =>
            {
                var dataSetHolder = ContainerBuilder.Container.Resolve<IDataSetHolder>();
                dataSetHolder.Store();
            });
        }
    }
}
