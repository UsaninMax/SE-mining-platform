using System.Collections.Generic;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.Sandbox.DataProviding.Transformers
{
    public interface ITransformer
    {
        List<Candle> Transform(IEnumerable<Tick> tiks, DataPredicate predicate);
        List<Tick> Transform(IEnumerable<DataTick> tiks, TickPredicate predicate);
    }
}
