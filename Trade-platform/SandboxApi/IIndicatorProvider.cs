using System.Collections.Generic;
using TradePlatform.SandboxApi.Models;

namespace TradePlatform.SandboxApi
{
    public interface IIndicatorProvider
    {
        List<Indicator> Get(IList<Candle> candles);
    }
}
