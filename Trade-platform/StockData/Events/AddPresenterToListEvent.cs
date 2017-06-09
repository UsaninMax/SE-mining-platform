using Prism.Events;
using TradePlatform.StockData.Models;

namespace TradePlatform.StockData.Events
{
    public class AddPresenterToListEvent : PubSubEvent<Instrument>
    {
    }
}
