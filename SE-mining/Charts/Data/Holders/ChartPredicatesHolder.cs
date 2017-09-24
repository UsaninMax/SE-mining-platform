using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using SE_mining_base.Charts.Data.Predicates.Basis;

namespace SEMining.Charts.Data.Holders
{
    public class ChartPredicatesHolder : IChartPredicatesHolder
    {
        private ISet<ChartPredicate> _storage = new HashSet<ChartPredicate>();

        public void Add(ChartPredicate predicate)
        {
            if(_storage.Contains(predicate)) {
                _storage.Remove(predicate);
            }
            _storage.Add(predicate);
        }

        public IEnumerable<ChartPredicate> GetAll()
        {
            return _storage;
        }

        public void Remove(ChartPredicate predicate)
        {
            _storage.Remove(predicate);
        }

        public void Reset()
        {
            _storage.Clear();
        }

        public void Add(IEnumerable<ChartPredicate> predicates)
        {
            predicates.ForEach(predicate => Add(predicate));
        }

        public IEnumerable<ChartPredicate> GetByChartId(string chartId)
        {
            return _storage.Where(x => x.ChartId.Equals(chartId));
        }
    }
}
