using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.Views;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public class ChartsBuilder : IChartsBuilder
    {
        public void Build(IEnumerable<PanelViewPredicate> configuration)
        {
            configuration.ForEach(chartPredicate =>
            {
                ContainerBuilder.Container.Resolve<ChartPanelView>( 
                    new DependencyOverride<IEnumerable<ChartViewPredicate>>(chartPredicate.ChartPredicates.Where(s => s.Ids.Count() != 0))).Show();
            });
        }
    }
}
