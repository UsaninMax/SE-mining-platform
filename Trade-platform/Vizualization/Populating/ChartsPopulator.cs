using System.Collections.Generic;
using TradePlatform.Vizualization.Builders;
using TradePlatform.Vizualization.Holders;
using TradePlatform.Vizualization.Populating.Predicates;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.Populating.Adaptors;
using LiveCharts.Wpf;
using TradePlatform.Vizualization.Populating.Providers;

namespace TradePlatform.Vizualization.Populating
{
    public class ChartsPopulator : IChartsPopulator
    {
        private IChartsHolder _chartHolder;
        private IChartsBuilder _chartBuilder;
        private IDataChartAdaptor _chartAdaptor;
        private IChartDataProvider _cahrtDataProvider;

        public ChartsPopulator()
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            _chartBuilder = ContainerBuilder.Container.Resolve<IChartsBuilder>();
            _chartAdaptor = ContainerBuilder.Container.Resolve<IDataChartAdaptor>();
            _cahrtDataProvider = ContainerBuilder.Container.Resolve<IChartDataProvider>();
        }

        public void Populate(CandlesDataPredicate predicate)
        {
            _chartHolder.Get(predicate.ChartId).Push(new OhlcSeries()
            {
               Values = _chartAdaptor.AdaptData(_cahrtDataProvider.Get(predicate))
            });
        }

        public void Populate(IndicatorDataPredicate predicate)
        {
            _chartHolder.Get(predicate.ChartId).Push(new LineSeries()
            {
                Values = _chartAdaptor.AdaptData(_cahrtDataProvider.Get(predicate))
            });
        }

        public void SetUpCharts(IEnumerable<Panel> configuration)
        {
            _chartHolder.Set(_chartBuilder.Build(configuration));
        }
    }
}
