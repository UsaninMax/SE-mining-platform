using System;
using System.Collections.Generic;
using System.Threading;
using TradePlatform.SandboxApi;
using TradePlatform.SandboxApi.DataProviding;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;

namespace TestSandboxModule
{
    public class TestSandbox : ISandbox
    {
        public void Before(CancellationToken token)
        {
            IList<Slice>  slices = new SliceProvider().Get(new List<IPredicate>
            {
                new DataPredicate.Builder()
                .ParentId("RTS")
                .NewId("RTS_5")
                .AccumulationPeriod(5000)
                .From(new DateTime(2016, 9, 7))
                .To(new DateTime(2016, 9, 11))
                .Build(),
                new DataPredicate.Builder()
                .ParentId("Si")
                .NewId("Si_5")
                .AccumulationPeriod(5000)
                .Build()
            });
        }

        public void Execution(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public void After(CancellationToken token)
        {
            throw new System.NotImplementedException();
        }
    }
}
