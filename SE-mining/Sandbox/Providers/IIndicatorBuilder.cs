using SE_mining_base.Sandbox.DataProviding.Predicates;

namespace SEMining.Sandbox.Providers
{
    public interface IIndicatorBuilder
    {
        IIndicatorProvider Build(IndicatorPredicate predicate);
    }
}
