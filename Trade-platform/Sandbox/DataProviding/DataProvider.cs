using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.DataSet.DataServices;
using TradePlatform.Sandbox.DataProviding.Checks;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.DataProviding.Transformers;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Providers;

namespace TradePlatform.Sandbox.DataProviding
{
    public class DataProvider : IDataProvider
    {
        private readonly IDataSetService _dataSetService;
        private readonly IPredicateChecker _predicateChecker;
        private readonly IInfoPublisher _infoPublisher;
        private readonly IIndicatorBuilder _indicatorBuilder;
        private IDictionary<string, IList<Tick>> _tiks = new Dictionary<string, IList<Tick>>();
        private IEnumerable<IData> _data = new List<IData>();
        private readonly ICollection<DataPredicate> _dataPredicates = new List<DataPredicate>();
        private readonly ICollection<IndicatorPredicate> _indicatorPredicates = new List<IndicatorPredicate>();
        private readonly ICollection<TickPredicate> _tickPredicate = new List<TickPredicate>();

        public DataProvider()
        {
            _predicateChecker = ContainerBuilder.Container.Resolve<IPredicateChecker>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _dataSetService = ContainerBuilder.Container.Resolve<IDataSetService>();
            _indicatorBuilder = ContainerBuilder.Container.Resolve<IIndicatorBuilder>();
        }

        public IList<IData> Get(ICollection<IPredicate> predicates, CancellationToken token)
        {
            if (token.IsCancellationRequested) { return null; }
            GatherPredicates(predicates);
            CheckPredicateStructure();
            GatherTickPredicates();
            if (token.IsCancellationRequested) { return null; }
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " constract tick data  " });
            _tickPredicate.ForEach(ConstructSeries);
            if (token.IsCancellationRequested) { return null; }
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " constract indicator data " });
            _indicatorPredicates.ForEach(ConstructSeries);
            if (token.IsCancellationRequested) { return null; }
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " constract data series " });
            _dataPredicates.ForEach(ConstructSeries);
            if (token.IsCancellationRequested) { return null; }
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " combine all data " });

            _tiks.Values.ForEach(x =>
            {
                _data = _data.Concat(x);
            });

            _tiks = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (token.IsCancellationRequested) { return null; }
            List<IData> asList = new List<IData>(_data);
            _data = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (token.IsCancellationRequested) { return null; }
            asList.Sort((x, y) => DateTime.Compare(x.Date(), y.Date()));
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " data is ready " });
            return asList;
        }

        private void ConstructSeries(DataPredicate predicate)
        {
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " build  " + predicate });
            ITransformer dataAggregator = ContainerBuilder.Container.Resolve<ITransformer>();
            _data = _data.Concat(dataAggregator.Transform(_tiks[predicate.ParentId], predicate));
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void ConstructSeries(TickPredicate predicate)
        {
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " build  " + predicate });
            ITransformer dataAggregator = ContainerBuilder.Container.Resolve<ITransformer>();
            _tiks.Add(predicate.Id, dataAggregator.Transform(_dataSetService.Get(predicate.Id), predicate));
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void ConstructSeries(IndicatorPredicate predicate)
        {
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " build  " + predicate });
            IIndicatorProvider provider = _indicatorBuilder.Build(predicate.Indicator); 
            ITransformer dataAggregator = ContainerBuilder.Container.Resolve<ITransformer>();
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " Build indicator  " + predicate });
            _data = _data.Concat(dataAggregator.Transform(_tiks[predicate.DataPredicate.ParentId], predicate.DataPredicate)
                .Select(c =>
                {
                    Indicator ind = provider.Get(c);
                    ind.SetId(predicate.Id);
                    return ind;
                }).ToList());
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void GatherTickPredicates()
        {
            ISet<string> parentIds = new HashSet<string>();
            _dataPredicates.ForEach(x => parentIds.Add(x.ParentId));
            _indicatorPredicates.ForEach(x => parentIds.Add(x.DataPredicate.ParentId));
            parentIds.ForEach(x => _tickPredicate.Add(ConstractTickPredicate(x)));
        }

        private TickPredicate ConstractTickPredicate(string parentId)
        {
            var predicate = new TickPredicate.Builder().NewId(parentId);

            DateTime from = DateTime.MaxValue;
            DateTime to = DateTime.MinValue;

            _dataPredicates.Where(x => x.ParentId.Equals(parentId)).ForEach(x =>
            {
                from = new DateTime(Math.Min(from.Ticks, x.From.Ticks));
                to = new DateTime(Math.Max(to.Ticks, x.To.Ticks));
            });

            _indicatorPredicates.Where(x => x.DataPredicate.ParentId.Equals(parentId)).ForEach(x =>
            {
                from = new DateTime(Math.Max(from.Ticks, x.DataPredicate.From.Ticks));
                to = new DateTime(Math.Min(to.Ticks, x.DataPredicate.To.Ticks));
            });

            return predicate.From(from).To(to).Build();
        }

        private void GatherPredicates(ICollection<IPredicate> predicates)
        {
            predicates.ForEach(x =>
            {
                if (x is DataPredicate)
                {
                    _dataPredicates.Add((DataPredicate)x);
                }
                if (x is IndicatorPredicate)
                {
                    _indicatorPredicates.Add((IndicatorPredicate)x);
                }
            });
        }

        private void CheckPredicateStructure()
        {
            if (!_dataPredicates.All(_predicateChecker.Check))
            {
                throw new Exception("Data predicate was not ready for use");
            }

            if (!_indicatorPredicates.All(_predicateChecker.Check))
            {
                throw new Exception("Indicator predicate was not ready for use");
            }
        }
    }
}
