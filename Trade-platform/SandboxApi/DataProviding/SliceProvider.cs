using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using TradePlatform.SandboxApi.DataProviding.Checks;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Info;
using TradePlatform.Commons.Info.Model.Message;
using TradePlatform.DataSet.DataServices;
using TradePlatform.StockData.Models;

namespace TradePlatform.SandboxApi.DataProviding
{
    public class SliceProvider : ISliceProvider
    {
        private readonly IDataSetService _dataSetService;
        private readonly IPredicateChecker _predicateChecker;
        private readonly IInfoPublisher _infoPublisher;
        private Queue<Queue<Tick>> _tiks = new Queue<Queue<Tick>>();
        private Queue<Queue<Tick>> _candles = new Queue<Queue<Tick>>();
        private Queue<Queue<Indicator>> _indicators = new Queue<Queue<Indicator>>();
        private readonly ICollection<DataPredicate> _dataPredicates = new List<DataPredicate>();
        private readonly ICollection<IndicatorPredicate> _indicatorPredicates = new List<IndicatorPredicate>();
        private readonly ICollection<TickPredicate> _tickPredicate = new List<TickPredicate>();

        public SliceProvider()
        {
            _predicateChecker = ContainerBuilder.Container.Resolve<IPredicateChecker>();
            _infoPublisher = ContainerBuilder.Container.Resolve<IInfoPublisher>();
            _dataSetService = ContainerBuilder.Container.Resolve<IDataSetService>();
        }

        public IList<Slice> Get(ICollection<IPredicate> predicates)
        {
            GatherPredicates(predicates);
            CheckPredicateStructure();
            GatherTickPredicates();

            _infoPublisher.PublishInfo(new SandboxInfo { Message = " constract data series " });
            _dataPredicates.ForEach(ConstructSeries);

            _infoPublisher.PublishInfo(new SandboxInfo { Message = " constract indicator data " });
            _indicatorPredicates.ForEach(ConstructSeries);
            _infoPublisher.PublishInfo(new SandboxInfo { Message = " add tick data for support calculation " });
            _tickPredicate.ForEach(ConstructSeries);

            _infoPublisher.PublishInfo(new SandboxInfo { Message = " constract slices of data " });
            return GroupSlices();
        }

        private IList<Slice> GroupSlices()
        {
            return null;
        }

        private void ConstructSeries(DataPredicate predicate)
        {
            IList<DataTick> tiks = _dataSetService.Get(predicate.ParentId);

        }

        private void ConstructSeries(TickPredicate predicate)
        {


        }

        private void ConstructSeries(IndicatorPredicate predicate)
        {


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

            DateTime from = DateTime.MinValue;
            DateTime to = DateTime.MaxValue;

            _dataPredicates.Where(x => x.ParentId.Equals(parentId)).ForEach(x =>
            {
                from = new DateTime(Math.Min(from.Millisecond, x.From.Millisecond));
                to = new DateTime(Math.Max(to.Millisecond, x.To.Millisecond));
            });

            _indicatorPredicates.Where(x => x.DataPredicate.ParentId.Equals(parentId)).ForEach(x =>
            {
                from = new DateTime(Math.Min(from.Millisecond, x.DataPredicate.From.Millisecond));
                to = new DateTime(Math.Max(to.Millisecond, x.DataPredicate.To.Millisecond));
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
            if (_dataPredicates.All(_predicateChecker.Check))
            {
                throw new Exception("Data predicate was not ready for use to buid data");
            }

            if (_indicatorPredicates.All(_predicateChecker.Check))
            {
                throw new Exception("Indicator predicate was not ready for use to buid indicator data");
            }
        }
    }
}
