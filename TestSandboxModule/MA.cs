using System;
using TradePlatform.Sandbox;
using TradePlatform.Sandbox.Models;

namespace TestSandboxModule
{
    public class MA : IIndicatorProvider
    {
        public Indicator Get(Candle candle)
        {
            return new Indicator.Builder()
                    .WithDate(DateTime.Now)
                    .WithValue(123)
                    .Build();
        }
    }
}
