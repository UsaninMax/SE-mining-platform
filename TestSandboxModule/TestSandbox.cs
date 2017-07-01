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
                .AccumulationPeriod(new TimeSpan(0,0,5))
                .From(new DateTime(2016, 9, 1))
                .To(new DateTime(2016, 9, 5))
                .Build(),
                new IndicatorPredicate.Builder()
                .NewId("MA")
                .Indicator(typeof(MA))
                .DataPredicate(new DataPredicate.Builder()
                        .NewId("Test")
                        .ParentId("RTS")
                        .AccumulationPeriod(new TimeSpan(0,0,5))
                        .From(new DateTime(2016, 9, 1))
                        .To(new DateTime(2016, 9, 5))
                        .Build())
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
