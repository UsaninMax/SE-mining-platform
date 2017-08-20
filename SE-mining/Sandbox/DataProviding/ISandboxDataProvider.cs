using System.Collections.Generic;
using System.Threading;
using SEMining.Sandbox.DataProviding.Predicates;
using SEMining.Sandbox.Models;

namespace SEMining.Sandbox.DataProviding
{
    public interface ISandboxDataProvider
    {
        IEnumerable<Slice> Get(IEnumerable<IPredicate> predicates, CancellationToken token);
    }
}
