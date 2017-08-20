using Prism.Events;
using SEMining.StockData.Models;

namespace SEMining.StockData.Events
{
    public class AddInstrumentToListEvent : PubSubEvent<Instrument>
    {
    }
}
