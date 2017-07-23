using System.Collections;
using System.Collections.Generic;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using TradePlatform.Vizualization.Builders;
using TradePlatform.Vizualization.Builders.Predicates;
using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ContainerBuilder.Initialize();
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();


            IChartsBuilder builder = new ChartsBuilder();
            IDictionary<string, IChartViewModel> charts = builder.Build(new List<Panel>
            {
                new Panel{Charts = new List<Chart>
                {
                    new Chart{Ids = new List<string>
                    {
                        "12_min",
                        "10_min"
                    }},
                    new Chart{Ids = new List<string>
                    {
                        "1222_min",
                        "10444_min"
                    }}
                }},
                new Panel{Charts = new List<Chart>
                {
                    new Chart{Ids = new List<string>
                    {
                        "133_min",
                        "143_min"
                    }}
                }}


            });

            charts["12_min"].Add(new ChartValues<OhlcPoint>
            {
                new OhlcPoint(32, 35, 30, 32),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(33, 38, 31, 37),
                new OhlcPoint(35, 42, 30, 40),
                new OhlcPoint(37, 40, 35, 38),
                new OhlcPoint(35, 38, 32, 33)
            });

            charts["133_min"].Add(new ChartValues<double> { 4, 6, 5, 2, 7, 4, 6, 5, 2, 7, 7, 4, 6, 5, 2, 7, 7, 4, 6, 5, 2, 7 });
        }
    }
}
