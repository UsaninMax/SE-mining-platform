using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using SEMining.Charts.Vizualization.Configurations;
using SEMining.Charts.Vizualization.ViewModels;
using Microsoft.Practices.ObjectBuilder2;

namespace SEMining.Charts.Vizualization.Dispatching
{
    public class ChartsConfigurationDispatcher : IChartsConfigurationDispatcher
    {
        public IDictionary<string, IChartViewModel> Dispatch(IEnumerable<PanelViewPredicate> configuration)
        {
            var dispatchedStructure = new Dictionary<string, IChartViewModel>();

            configuration
                .SelectMany(x => x.ChartPredicates)
                .ForEach(predicate =>
                {
                    var model = CreateModel(predicate);
                    predicate.Ids.ForEach(id =>
                    {
                        dispatchedStructure.Add(id, model);
                    });
                });
            return dispatchedStructure;
        }

        private IChartViewModel CreateModel(ChartViewPredicate predicate)
        {
            if (predicate is DateChartViewPredicate)
            {
                var custedPredicate = predicate as DateChartViewPredicate;
                return ContainerBuilder.Container.Resolve<IChartViewModel>("DateChartViewModel",
                    new DependencyOverride<TimeSpan>(custedPredicate.XAxis));
            }
            return ContainerBuilder.Container.Resolve<IChartViewModel>("IndexChartViewModel");
        }
    }
}
