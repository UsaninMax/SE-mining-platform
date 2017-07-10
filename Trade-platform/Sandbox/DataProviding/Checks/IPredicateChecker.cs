using TradePlatform.Sandbox.DataProviding.Predicates;

namespace TradePlatform.Sandbox.DataProviding.Checks
{
    public interface IPredicateChecker
    {
        bool Check(DataPredicate predicate);
        bool Check(IndicatorPredicate predicate);
    }
}
