using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Vizualization.Views
{
    public partial class ChartPanelView
    {
        public ChartPanelView(IEnumerable<Tuple<IChartViewModel, ChartViewPredicate>> settings )
        {
            InitializeComponent();
            settings.ForEach(setting =>
            {
                if (setting.Item2 is DateChartViewPredicate)
                {
                    ChartStack.Children.Add(new DateChartView(setting.Item1) { Height = setting.Item2.YSize });
                }
                else if (setting.Item2 is IndexChartViewPredicate)
                {
                    ChartStack.Children.Add(new IndexChartView(setting.Item1) { Height = setting.Item2.YSize });
                }
            });
        }
    }
}
