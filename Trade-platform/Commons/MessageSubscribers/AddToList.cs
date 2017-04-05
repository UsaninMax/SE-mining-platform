using Prism.Events;
using TradePlatform.StockDataDownload.Presenters;

namespace TradePlatform.Commons.MessageSubscribers
{
    class AddToList<TDounloadInstrumentPresenter> : PubSubEvent<TDounloadInstrumentPresenter>
    {
    }
}
