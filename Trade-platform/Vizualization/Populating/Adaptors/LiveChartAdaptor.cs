using System.Collections.Generic;
using LiveCharts;
using TradePlatform.Sandbox.Models;
using System.Linq;

namespace TradePlatform.Vizualization.Populating.Adaptors
{
    public class LiveChartAdaptor : IDataChartAdaptor
    {
        public ChartValues<Candle> AdaptData(IList<Candle> values)
        {
           return new ChartValues<Candle>(values.Take(100));
        }

        public ChartValues<double> AdaptData(IList<Indicator> values)
        {
            return new ChartValues<double>(values.Select(x => x.Value));
        }

        public IList<string> GetLabels(IList<Indicator> values)
        {
            return values.Select(x => x.Date().ToString("dd/MM/yyyy HH:mm:ss")).ToList();
        }

        public IList<string> GetLabels(IList<Candle> values)
        {
            return values.Select(x => x.Date().ToString("dd/MM/yyyy HH:mm:ss")).ToList();
        }
    }
}
