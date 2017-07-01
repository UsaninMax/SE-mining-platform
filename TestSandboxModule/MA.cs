using System;
using System.Collections.Generic;
using TradePlatform.SandboxApi;
using TradePlatform.SandboxApi.Models;

namespace TestSandboxModule
{
    public class MA : IIndicatorProvider
    {
        public List<Indicator> Get(IList<Candle> candles)
        {
            return new List<Indicator> {
                new Indicator.Builder()
                .WithDate(DateTime.Now)
                .WithValue(123)
                .Build()
            };
        }
    }
}
