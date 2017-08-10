using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.Views;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public class ChartsBuilder : IChartsBuilder
    {
        public void Build(IEnumerable<PanelViewPredicate> configuration)
        {
            configuration.ForEach(chartPredicate =>
            {
                ContainerBuilder.Container.Resolve<ChartPanelView>( 
                    new DependencyOverride<IEnumerable<ChartViewPredicate>>(chartPredicate.Charts.Where(s => s.Ids.Count() != 0))).Show();
            });
        }
    }
}
