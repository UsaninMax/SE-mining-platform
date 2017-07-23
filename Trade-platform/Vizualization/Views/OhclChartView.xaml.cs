using System.Windows.Controls;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class OhclChartView : UserControl
    {
        public OhclChartView(IOhclChartViewModel model)
        {
            InitializeComponent();
            DataContext = model;
        }
    }
}
