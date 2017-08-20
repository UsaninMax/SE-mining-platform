using SEMining.Sandbox.DataProviding.Predicates;

namespace SEMining.Sandbox.Providers
{
    public interface IIndicatorBuilder
    {
        IIndicatorProvider Build(IndicatorPredicate predicate);
    }
}
