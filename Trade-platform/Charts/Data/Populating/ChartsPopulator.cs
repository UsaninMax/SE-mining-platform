using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Charts.Data.Holders;
using TradePlatform.Charts.Data.Predicates.Basis;
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
        private readonly IChartDataProvider _chartDataProvider;
        private readonly IChartProxy _chartProxy;
        private readonly IChartPredicatesHolder _chartPredicatesHolder;

        public ChartsPopulator(IEnumerable<PanelViewPredicate> configuration)
        {
            _chartHolder = ContainerBuilder.Container.Resolve<IChartsHolder>();
            _chartDataProvider = ContainerBuilder.Container.Resolve<IChartDataProvider>();
            _chartProxy = ContainerBuilder.Container.Resolve<IChartProxy>();
            _chartPredicatesHolder = ContainerBuilder.Container.Resolve<IChartPredicatesHolder>();
            _chartHolder.Set(ContainerBuilder.Container.Resolve<IChartsConfigurationDispatcher>().Dispatch(configuration));
        }

        public void Populate()
        {
            _chartPredicatesHolder.GetAll().ForEach(predicate => _chartProxy.Clear(_chartHolder.Get(predicate.ChartId)));
            _chartPredicatesHolder.GetAll().ForEach(predicate =>
            {
                if (predicate is IndexChartPredicate)
                {
                    Populate(_chartHolder.Get(predicate.ChartId),
                        predicate,
                        ((IndexChartPredicate)predicate).From,
                        ((IndexChartPredicate)predicate).To);

                }
                else if (predicate is DateChartPredicate)
                {
                    Populate(_chartHolder.Get(predicate.ChartId),
                        predicate,
                        ((DateChartPredicate)predicate).From,
                        ((DateChartPredicate)predicate).To);
                }
            });
        }

        public void Populate(IChartViewModel model, DateTime from, DateTime to)
        {
            _chartProxy.Clear(model);
            _chartHolder.Get(model).ForEach(chartId =>
            {
                _chartPredicatesHolder.GetByChartId(chartId).ForEach(predicate =>
                {
                    ((DateChartPredicate)predicate).From = from;
                    ((DateChartPredicate)predicate).To = to;
                    Populate(model, predicate, from, to);
                });
            });
        }

        public void Populate(IChartViewModel model, int from, int to)
        {
            _chartProxy.Clear(model);
            _chartHolder.Get(model).ForEach(chartId =>
            {
                _chartPredicatesHolder.GetByChartId(chartId).ForEach(predicate =>
                {
                    ((IndexChartPredicate)predicate).From = from;
                    ((IndexChartPredicate)predicate).To = to;
                    Populate(model, predicate, from, to);
                });
            });
        }

        public void Populate(IChartViewModel model, ChartPredicate predicate, DateTime from, DateTime to)
        {
            if (predicate.CasType == typeof(Indicator) && predicate is IExistDataStorage)
            {
                _chartProxy.Push(model, _chartDataProvider.GetExistStorageData<Indicator>(predicate.InstrumentId)
                    .Where(m => (from == DateTime.MinValue || m.Date() >= from) && (to == DateTime.MinValue || m.Date() <= to)).ToList(), predicate);
            }
            else if (predicate.CasType == typeof(Indicator) && predicate is ICustomStorage)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<Indicator>(predicate.InstrumentId)
                    .Where(m => (from == DateTime.MinValue || m.Date() >= from) && (to == DateTime.MinValue || m.Date() <= to)).ToList(), predicate);
            }
            else if (predicate.CasType == typeof(Candle) && predicate is IExistDataStorage)
            {
                _chartProxy.Push(model, _chartDataProvider.GetExistStorageData<Candle>(predicate.InstrumentId)
                    .Where(m => (from == DateTime.MinValue || m.Date() >= from) && (to == DateTime.MinValue || m.Date() <= to)).ToList(), predicate);
            }
            else if (predicate.CasType == typeof(Candle) && predicate is ICustomStorage)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<Candle>(predicate.InstrumentId)
                        .Where(m => (from == DateTime.MinValue || m.Date() >= from) && (to == DateTime.MinValue || m.Date() <= to)).ToList(), predicate);
            }
            else if (predicate.CasType == typeof(Transaction) && predicate is ICustomStorage)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<Transaction>(predicate.InstrumentId)
                    .Where(m => (from == DateTime.MinValue || m.Date >= from) && (to == DateTime.MinValue || m.Date <= to)).ToList());
            }
        }

        public void Populate(IChartViewModel model, ChartPredicate predicate, int from, int to)
        {
            if (predicate.CasType == typeof(double) && predicate is ICustomStorage)
            {
                _chartProxy.Push(model, _chartDataProvider.GetCustomStorageData<double>(predicate.InstrumentId).Skip(from).Take(to - from).ToList(), predicate);
            }
        }
    }
}
