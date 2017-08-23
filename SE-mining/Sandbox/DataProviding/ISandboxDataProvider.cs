using System.Collections.Generic;
using System.Threading;
using SE_mining_base.Sandbox.DataProviding.Predicates;
using SE_mining_base.Sandbox.Models;

namespace SEMining.Sandbox.DataProviding
{
    public interface ISandboxDataProvider
    {
        IEnumerable<Slice> Get(IEnumerable<IPredicate> predicates, CancellationToken token);
    }
}
