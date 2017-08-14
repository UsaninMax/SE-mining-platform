using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox
{
    public interface IIndicatorProvider
    {
        void SetUpParameters(IDictionary<string, object> parameters);
        void Initialize();
        Indicator Get(Candle candle);
    }
}
