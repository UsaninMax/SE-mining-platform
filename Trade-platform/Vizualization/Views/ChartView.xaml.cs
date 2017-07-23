using System.Windows.Controls;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class ChartView : UserControl
    {

        public ChartView(IChartViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
        
    }
}
