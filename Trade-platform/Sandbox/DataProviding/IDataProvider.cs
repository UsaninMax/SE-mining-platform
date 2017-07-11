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
        IList<Tuple<DateTime, IEnumerable<IData>, IEnumerable<Tick>>> Get(ICollection<IPredicate> predicates, CancellationToken token);
    }
}
