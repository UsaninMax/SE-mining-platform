using System.Windows;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class ChartsView : Window
    {
        public ChartsView()
        {
            InitializeComponent();

            ChartStack.Children.Add(new OhclChartView(ContainerBuilder.Container.Resolve<IOhclChartViewModel>()));
            ChartStack.Children.Add(new OhclChartView(ContainerBuilder.Container.Resolve<IOhclChartViewModel>()));
            ChartStack.Children.Add(new OhclChartView(ContainerBuilder.Container.Resolve<IOhclChartViewModel>()));
        }
    }
}
