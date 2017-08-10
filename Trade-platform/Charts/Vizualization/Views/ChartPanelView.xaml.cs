using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.Holders;
using Microsoft.Practices.Unity;

namespace TradePlatform.Charts.Vizualization.Views
{
    public partial class ChartPanelView
    {
        public ChartPanelView(IEnumerable<ChartViewPredicate> chartPredicates )
        {
            InitializeComponent();
            IChartsHolder chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            chartPredicates.ForEach(predicate =>
            {
                var model = chartHolder.Get(predicate);
                if (predicate is DateChartViewPredicate)
                {
                    ChartStack.Children.Add(new DateChartView(model) { Height = predicate.YSize });
                }
                else if (predicate is IndexChartViewPredicate)
                {
                    ChartStack.Children.Add(new IndexChartView(model) { Height = predicate.YSize });
                }
            });
        }
    }
}
