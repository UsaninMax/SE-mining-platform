using System.Collections.Generic;
using SEMining.Sandbox.DataProviding.Predicates;
using SEMining.Sandbox.Models;
using SEMining.StockData.Models;

namespace SEMining.Sandbox.DataProviding.Transformers
{
    public interface ITransformer
    {
        List<Candle> Transform(IEnumerable<Tick> tiks, DataPredicate predicate);
        List<Tick> Transform(IEnumerable<DataTick> tiks, TickPredicate predicate);
    }
}
