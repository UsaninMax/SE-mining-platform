using TradePlatform.Vizualization.ViewModels;

namespace TradePlatform.Vizualization.Populating
{
    public interface IChartsPopulator
    {
        void Populate();
        void Populate(IChartViewModel model, int index);
    }
}
