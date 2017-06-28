using System.Collections.Generic;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using TradePlatform.StockData.Models;

namespace TradePlatform.SandboxApi.DataProviding.Transformers
{
    public interface ITransformer
    {
        Queue<Candle> Transform(List<DataTick> tiks, DataPredicate predicate);
        Queue<Tick> Transform(List<DataTick> tiks, TickPredicate predicate);
    }
}
