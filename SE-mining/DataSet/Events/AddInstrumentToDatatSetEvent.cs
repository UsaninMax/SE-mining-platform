using System.Collections.Generic;
using Prism.Events;
using SEMining.StockData.Models;

namespace SEMining.DataSet.Events
{
    public class AddInstrumentToDatatSetEvent : PubSubEvent<IEnumerable<Instrument>>
    {
    }
}
