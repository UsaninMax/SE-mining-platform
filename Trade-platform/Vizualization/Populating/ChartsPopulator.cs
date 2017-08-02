using TradePlatform.Vizualization.Holders;
using TradePlatform.Vizualization.Populating.Predicates;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Populating.Providers;
using TradePlatform.Vizualization.Charts;
using TradePlatform.Vizualization.Builders;
using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;

namespace TradePlatform.Vizualization.Populating
{
    public class ChartsPopulator : IChartsPopulator
    {
        private readonly IChartsHolder _chartHolder;
        private readonly IChartsConfigurationDispatcher _configurationDispatcher;
        private readonly IChartDataProvider _cahrtDataProvider;
        private readonly ChartProxy _chartProxy;

        public ChartsPopulator(IEnumerable<PanelViewPredicate> configuration)
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            _configurationDispatcher = ContainerBuilder.Container.Resolve<IChartsConfigurationDispatcher>();
            _cahrtDataProvider = ContainerBuilder.Container.Resolve<IChartDataProvider>();
            _chartProxy = ContainerBuilder.Container.Resolve<ChartProxy>();
            _chartHolder.Set(_configurationDispatcher.Dispatch(configuration));
        }

        public void Populate(CandlesDataPredicate predicate)
        {
            _chartProxy.Push(_chartHolder.Get(predicate.ChartId), _cahrtDataProvider.Get(predicate));
        }

        public void Populate(IndicatorDataPredicate predicate)
        {
            _chartProxy.Push(_chartHolder.Get(predicate.ChartId), _cahrtDataProvider.Get(predicate));
        }
    }
}
