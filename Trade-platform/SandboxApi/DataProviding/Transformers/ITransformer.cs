using System.Collections.Generic;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.SandboxApi.DataProviding.Transformers
{
    public interface ITransformer
    {
        List<Candle> Transform(IList<Tick> tiks, DataPredicate predicate);
        List<Tick> Transform(List<DataTick> tiks, TickPredicate predicate);
    }
}
