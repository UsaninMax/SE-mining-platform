using System;
using TradePlatform.Charts.Vizualization.ViewModels;

namespace TradePlatform.Charts.Data.Populating
{
    public interface IChartsPopulator
    {
        void Populate();
        void Populate(IChartViewModel model, DateTime from, DateTime to);
        void Populate(IChartViewModel model, int from, int to);
    }
}
