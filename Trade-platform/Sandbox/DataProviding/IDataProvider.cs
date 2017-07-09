using System.Collections.Generic;
using System.Threading;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.DataProviding
{
    public interface IDataProvider
    {
        IList<IData> Get(ICollection<IPredicate> predicates, CancellationToken token);
    }
}
