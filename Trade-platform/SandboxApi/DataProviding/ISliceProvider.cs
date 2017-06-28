using System.Collections.Generic;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;

namespace TradePlatform.SandboxApi.DataProviding
{
    public interface ISliceProvider
    {
        IList<Slice> Get(ICollection<IPredicate> predicates);
    }
}
