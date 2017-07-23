using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.ViewModels;
using TradePlatform.Vizualization.Views;

namespace TradePlatform.Vizualization.Builders
{
    public class ChartsBuilder : IChartsBuilder
    {
        public IDictionary<string, IChartViewModel> Build(IEnumerable<Panel> configuration)
        {
            IDictionary<string, IChartViewModel> charts = new Dictionary<string, IChartViewModel>();

            configuration.ForEach(x =>
            {
                ContainerBuilder.Container.Resolve<ChartPanelView>(new DependencyOverride<IEnumerable<IChartViewModel>>(x.Charts.Select(y =>
                {
                    var model = ContainerBuilder.Container.Resolve<IChartViewModel>();

                    y.Ids.ForEach(z =>
                    {
                        charts.Add(z, model);
                    });
                    return model;
                }).ToList())).Show();
            });

            return charts;
        }
    }
}
