using System;
using System.Collections.Generic;
using System.Threading;
using Castle.Core;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox.DataProviding
{
    public interface IDataProvider
    {
        IList<Pair<DateTime, IEnumerable<IData>>> Get(ICollection<IPredicate> predicates, CancellationToken token);
    }
}
