using System;

namespace TradePlatform.Sandbox.Providers
{
    public interface IIndicatorBuilder
    {
        IIndicatorProvider Build(Type type);
    }
}
