using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Data.Holders;
using TradePlatform.Charts.Data.Predicates;
using TradePlatform.Charts.Data.Providers;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Charts.Vizualization.Dispatching;
using TradePlatform.Charts.Vizualization.Holders;
using TradePlatform.Charts.Vizualization.ViewModels;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Charts.Data.Populating
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
                Populate(_chartHolder.Get(predicate.ChartId), predicate);
            });
        }

        public void Populate(IChartViewModel model, int index)
        {
            _chartProxy.Clear(model);
            _chartHolder.Get(model).ForEach(chartId =>
            {
                _chartPredicatesHolder.Get(chartId).ForEach(predicate =>
               {
                   predicate.Index = index;
                   Populate(_chartHolder.Get(predicate.ChartId), predicate);
               });
            });
        }

        private void Populate(IChartViewModel model, ChartPredicate predicate)
        {
            if (predicate is ExistCandlePredicate)
            {
                _chartProxy.Push(model, _chartDataProvider.GetExistStorageData<Candle>(predicate), predicate);
            }

            if (predicate is CustomCandlePredicate)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<Candle>(predicate), predicate);
            }

            if (predicate is CustomIndicatorPredicate)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<Indicator>(predicate), predicate);
            }

            if (predicate is CustomDoublePredicate)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<double>(predicate), predicate);
            }

            if (predicate is ExistIndicatorPredicate)
            {
                _chartProxy.Push(model, _chartDataProvider.GetExistStorageData<Indicator>(predicate), predicate);
            }

            if (predicate is CustomTransactionPredicate)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<Transaction>(predicate));
            }
        }
    }
}
