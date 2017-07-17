using TradePlatform.Sandbox.DataProviding.Predicates;

namespace TradePlatform.Sandbox.Providers
{
    public interface IIndicatorBuilder
    {
        IIndicatorProvider Build(IndicatorPredicate predicate);
    }
}
