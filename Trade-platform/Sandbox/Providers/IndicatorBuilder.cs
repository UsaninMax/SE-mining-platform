using System;
using TradePlatform.Sandbox.DataProviding.Predicates;

namespace TradePlatform.Sandbox.Providers
{
    public class IndicatorBuilder : IIndicatorBuilder
    {
        public IIndicatorProvider Build(IndicatorPredicate predicate)
        {
            IIndicatorProvider provider = Activator.CreateInstance(predicate.Indicator) as IIndicatorProvider;
            if (provider == null)
            {
                throw new Exception(predicate.GetType() + "Indicator cannot be instantiated");
            }
            provider.SetUpParameters(predicate.Parameters);
            provider.Initialize();
            return provider;
        }
    }
}
