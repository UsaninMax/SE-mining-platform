
using System.Collections.Generic;
using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Vizualization.ViewModels
{
    public interface IChartViewModel
    {
        void Push(IList<Indicator> values);
        void Push(IList<Candle> values);
        void Push(IList<double> values);
        void Push(IList<Transaction> values);
        void ClearAll();
    }
}
