using System.Collections.Generic;
using System.Threading;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;

namespace TradePlatform.SandboxApi.DataProviding
{
    public interface IDataProvider
    {
        IList<IData> Get(ICollection<IPredicate> predicates, CancellationToken token);
    }
}
