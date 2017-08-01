using System.Collections.Generic;
using TradePlatform.Vizualization.Builders;
using TradePlatform.Vizualization.Holders;
using TradePlatform.Vizualization.Populating.Predicates;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Adaptors;
using TradePlatform.Vizualization.Populating.Providers;

namespace TradePlatform.Vizualization.Charts
{
    public class ChartsPopulator : IChartsPopulator
    {
        private readonly IChartsHolder _chartHolder;
        private readonly IChartsBuilder _chartBuilder;
        private readonly IDataChartAdaptor _chartAdaptor;
        private readonly IChartDataProvider _cahrtDataProvider;
        private readonly ChartProxy _chartProxy;

        public ChartsPopulator()
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            _chartBuilder = ContainerBuilder.Container.Resolve<IChartsBuilder>();
            _chartAdaptor = ContainerBuilder.Container.Resolve<IDataChartAdaptor>();
            _cahrtDataProvider = ContainerBuilder.Container.Resolve<IChartDataProvider>();
            _chartProxy = ContainerBuilder.Container.Resolve<ChartProxy>();
        }

        public void Populate(CandlesDataPredicate predicate)
        {
            _chartProxy.Push(predicate.ChartId, _chartAdaptor.AdaptData(_cahrtDataProvider.Get(predicate)));
        }

        public void Populate(IndicatorDataPredicate predicate)
        {
            _chartProxy.Push(predicate.ChartId, _chartAdaptor.AdaptData(_cahrtDataProvider.Get(predicate)));
        }

        public void SetUpCharts(IEnumerable<Panel> configuration)
        {
            _chartHolder.Set(_chartBuilder.Build(configuration));
            _chartProxy.ShowCharts(configuration);
        }
    }
}
