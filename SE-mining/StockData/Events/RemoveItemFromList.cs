using Prism.Events;
using SEMining.StockData.Presenters;

namespace SEMining.StockData.Events
{
    public class RemovePresenterFromListEvent : PubSubEvent<IDounloadInstrumentPresenter>
    {
    }
}
