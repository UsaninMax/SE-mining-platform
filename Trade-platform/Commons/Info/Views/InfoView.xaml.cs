using System.Windows;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Info.ViewModels;

namespace TradePlatform.Commons.Info.Views
{ 
    public partial class InfoView : Window
    {
        public InfoView()
        {
            InitializeComponent();
            this.DataContext = ContainerBuilder.Container.Resolve<IInfoViewModel>();
        }
    }
}
