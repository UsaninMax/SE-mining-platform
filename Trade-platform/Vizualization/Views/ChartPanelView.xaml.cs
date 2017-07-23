using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Views
{
    public partial class ChartPanelView
    {
        public ChartPanelView(IEnumerable<IChartViewModel> chart)
        {
            InitializeComponent();

            chart.ForEach(x =>
            {
                ChartStack.Children.Add(new ChartView(x));
            });
        }
    }
}
