using Prism.Events;
using TradePlatform.StockData.Presenters;

namespace TradePlatform.StockData.Events
{
    public class RemovePresenterFromListEvent : PubSubEvent<IDounloadInstrumentPresenter>
    {
    }
}
