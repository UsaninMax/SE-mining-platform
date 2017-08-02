using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Holders;
using TradePlatform.Vizualization.ViewModels;
using TradePlatform.Vizualization.Views;

namespace TradePlatform.Vizualization.Builders
{
    public class ChartsBuilder : IChartsBuilder
    {
        private IChartsHolder _chartHolder;

        public ChartsBuilder ()
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
        }

        public void Build(IEnumerable<PanelViewPredicate> configuration)
        {
            configuration.ForEach(x =>
            {
                ContainerBuilder.Container.Resolve<ChartPanelView>( 
                    new DependencyOverride<IEnumerable<Tuple<IChartViewModel, ChartViewPredicate>>>(
                        x.Charts.Where(s => s.Ids.Count() != 0)
                        .Select(y =>
                        {
                            return new Tuple<IChartViewModel, ChartViewPredicate>(_chartHolder.Get(y.Ids.First()), y);
                        }).ToList())).Show();
            });
        }
    }
}
