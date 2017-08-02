
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Push(IList<Indicator> values);
        void Push(IList<Candle> values);
        void ClearAll();
    }
}
