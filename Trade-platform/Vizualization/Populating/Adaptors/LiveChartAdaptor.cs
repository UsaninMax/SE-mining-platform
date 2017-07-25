using System;
using System.Collections.Generic;
using LiveCharts;
using TradePlatform.Sandbox.Models;
using System.Linq;
using LiveCharts.Defaults;

namespace TradePlatform.Vizualization.Populating.Adaptors
{
    public class LiveChartAdaptor : IDataChartAdaptor
    {
        public ChartValues<OhlcPoint> AdaptData(IList<Candle> values)
        {
           return new ChartValues<OhlcPoint>(values.Select(x => new OhlcPoint(x.Open, x.High, x.Low, x.Close)));
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
