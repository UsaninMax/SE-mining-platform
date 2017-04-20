using Prism.Events;

namespace TradePlatform.StockDataDownload.MessageEvents
{
    public class RemoveFromList<IDounloadInstrumentPresenter> : PubSubEvent<IDounloadInstrumentPresenter>
    {
    }
}
