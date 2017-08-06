using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.Holders;
using TradePlatform.Charts.Vizualization.ViewModels;
using ChartPanelView = TradePlatform.Charts.Vizualization.Views.ChartPanelView;

namespace TradePlatform.Charts.Vizualization.Dispatching
{
    public class ChartsBuilder : IChartsBuilder
    {
        private readonly IChartsHolder _chartHolder;

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
                        .Select(predicate => new Tuple<IChartViewModel, ChartViewPredicate>(_chartHolder.Get(predicate.Ids.First()), predicate))
                        .ToList())).Show();
            });
        }
    }
}
