using System;

namespace TradePlatform.Sandbox.Providers
{
    public class IndicatorBuilder : IIndicatorBuilder
    {
        public IIndicatorProvider Build(Type type)
        {
            IIndicatorProvider provider = Activator.CreateInstance(type) as IIndicatorProvider;
            if (provider == null)
            {
                throw new Exception(type + "Indicator cannot be instantiated");
            }
            return provider;
        }
    }
}
