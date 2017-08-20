using Microsoft.Practices.Unity;
using SEMining.Commons.Info.ViewModels;

namespace SEMining.Commons.Info.Views
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
