﻿using LiveCharts;
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Vizualization.Populating.Adaptors
{
    public interface IDataChartAdaptor
    {
        ChartValues<Candle> AdaptData(IList<Candle> values);
        ChartValues<double> AdaptData(IList<Indicator> values);
        IList<string> GetLabels(IList<Indicator> values);
        IList<string> GetLabels(IList<Candle> values);

    }
}