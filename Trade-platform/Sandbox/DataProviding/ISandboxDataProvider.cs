using System.Collections.Generic;
using System.Threading;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.DataProviding
{
    public interface ISandboxDataProvider
    {
        IEnumerable<Slice> Get(IEnumerable<IPredicate> predicates, CancellationToken token);
    }
}
