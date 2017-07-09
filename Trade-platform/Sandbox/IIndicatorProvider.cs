using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox
{
    public interface IIndicatorProvider
    {
        Indicator Get(Candle candle);
    }
}
