using System;
using SEMining.Charts.Vizualization.ViewModels;

namespace SEMining.Charts.Data.Populating
{
    public interface IChartsPopulator
    {
        void Populate();
        void Populate(IChartViewModel model, DateTime from, DateTime to);
        void Populate(IChartViewModel model, int from, int to);
    }
}
