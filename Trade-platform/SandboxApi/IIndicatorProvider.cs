using TradePlatform.SandboxApi.Models;

namespace TradePlatform.SandboxApi
{
    public interface IIndicatorProvider
    {
        Indicator Get(Candle candle);
    }
}
