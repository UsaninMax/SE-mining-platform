using Microsoft.Practices.ObjectBuilder2;
using System.Collections.Generic;
using TradePlatform.Vizualization.Populating.Predicates;

namespace TradePlatform.Vizualization.Populating.Holders
{
    public class ChartPredicatesHolder : IChartPredicatesHolder
    {
        private ISet<ChartPredicate> _storage = new HashSet<ChartPredicate>();

        public void Update(ChartPredicate predicate)
        {
            if(_storage.Contains(predicate)) {
                _storage.Remove(predicate);
            }
            _storage.Add(predicate);
        }

        public ICollection<ChartPredicate> GetAll()
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

        public void Set(ICollection<ChartPredicate> predicates)
        {
            predicates.ForEach(predicate => Update(predicate));
        }
    }
}
