using TradePlatform.Vizualization.Holders;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Populating.Providers;
using TradePlatform.Vizualization.Charts;
using System.Collections.Generic;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Holders;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating
{
    public class ChartsPopulator : IChartsPopulator
    {
        private readonly IChartsHolder _chartHolder;
        private readonly IChartsConfigurationDispatcher _configurationDispatcher;
        private readonly IChartDataProvider _chartDataProvider;
        private readonly ChartProxy _chartProxy;
        private readonly IChartPredicatesHolder _chartPredicatesHolder;

        public ChartsPopulator(IEnumerable<PanelViewPredicate> configuration)
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            _configurationDispatcher = ContainerBuilder.Container.Resolve<IChartsConfigurationDispatcher>();
            _chartDataProvider = ContainerBuilder.Container.Resolve<IChartDataProvider>();
            _chartProxy = ContainerBuilder.Container.Resolve<ChartProxy>();
            _chartPredicatesHolder = ContainerBuilder.Container.Resolve<IChartPredicatesHolder>();
            _chartHolder.Set(_configurationDispatcher.Dispatch(configuration));
        }

        public void Populate()
        {
            _chartPredicatesHolder.GetAll().ForEach(predicate => _chartProxy.Clear(_chartHolder.Get(predicate.ChartId)));
            _chartPredicatesHolder.GetAll().ForEach(predicate =>
            {

                if (predicate is CandlesDataPredicate)
                {
                    _chartProxy.Push(_chartHolder.Get(predicate.ChartId), _chartDataProvider.Get(predicate as CandlesDataPredicate));
                }

                if (predicate is IndicatorDataPredicate)
                {
                    _chartProxy.Push(_chartHolder.Get(predicate.ChartId), _chartDataProvider.Get(predicate as IndicatorDataPredicate));
                }
            });
        }
    }
}
