using SE_mining_base.Sandbox.DataProviding.Predicates;

namespace SEMining.Sandbox.DataProviding.Checks
{
    public interface IPredicateChecker
    {
        bool Check(DataPredicate predicate);
        bool Check(IndicatorPredicate predicate);
    }
}
