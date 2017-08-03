using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Vizualization.ViewModels;
using TradePlatform.Vizualization.Builders.Predicates;
using System;

namespace TradePlatform.Vizualization.Views
{
    public partial class ChartPanelView
    {
        public ChartPanelView(IEnumerable<Tuple<IChartViewModel, ChartViewPredicate>> chart )
        {
            InitializeComponent();

            chart.ForEach(x =>
            {
                ChartView view = new ChartView(x.Item1);
                view.Height = x.Item2.YSize;
                ChartStack.Children.Add(view);
            });
        }
    }
}
