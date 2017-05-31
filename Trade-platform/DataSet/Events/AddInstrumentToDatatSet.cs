using System.Collections.Generic;
using Prism.Events;
using TradePlatform.StockData.Models;

namespace TradePlatform.DataSet.Events
{
    public class AddInstrumentToDatatSet : PubSubEvent<IList<Instrument>>
    {
    }
}
