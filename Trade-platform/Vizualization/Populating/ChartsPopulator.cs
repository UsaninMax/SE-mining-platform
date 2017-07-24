using System;
using System.Collections.Generic;
using TradePlatform.Vizualization.Builders;
using TradePlatform.Vizualization.Holders;
using TradePlatform.Vizualization.Populating.Predicates;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Builders.Predicates;

namespace TradePlatform.Vizualization.Populating
{
    public class ChartsPopulator : IChartsPopulator
    {
        private IChartsHolder _chartHolder;
        private IChartsBuilder _chartBuilder;

        public ChartsPopulator()
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            _chartBuilder = ContainerBuilder.Container.Resolve<IChartsBuilder>();
        }

        public void Populate(IEnumerable<ChartPredicate> predicates)
        {
            throw new NotImplementedException();
        }

        public void SetUpCharts(IEnumerable<Panel> configuration)
        {
            _chartHolder.Set(_chartBuilder.Build(configuration));
        }
    }
}
