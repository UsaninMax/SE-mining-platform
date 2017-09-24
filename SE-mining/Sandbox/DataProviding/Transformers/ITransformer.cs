using System.Collections.Generic;
using SEMining.StockData.Models;
using SE_mining_base.Sandbox.DataProviding.Predicates;
using SE_mining_base.Sandbox.Models;

namespace SEMining.Sandbox.DataProviding.Transformers
{
    public interface ITransformer
    {
        List<Candle> Transform(IEnumerable<Tick> tiks, DataPredicate predicate);
        List<Tick> Transform(IEnumerable<DataTick> tiks, TickPredicate predicate);
    }
}
