using TradePlatform.SandboxApi.DataProviding.Predicates;

namespace TradePlatform.SandboxApi.DataProviding.Checks
{
    public interface IPredicateChecker
    {
        bool Check(DataPredicate predicate);
        bool Check(IndicatorPredicate predicate);
    }
}
