using Microsoft.Practices.Unity;
using TradePlatform.Commons.Info.ViewModels;

namespace TradePlatform.Commons.Info.Views
{ 
    public partial class InfoView
    {
        public InfoView()
        {
            InitializeComponent();
            DataContext = ContainerBuilder.Container.Resolve<IInfoViewModel>();
        }
    }
}
