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
            IList<Slice> slices = new SliceProvider().Get(new List<IPredicate>
            {
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_1")
                    .AccumulationPeriod(new TimeSpan(0,0,1))
                    .From(new DateTime(2014, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_5")
                    .AccumulationPeriod(new TimeSpan(0,0,5))
                    .From(new DateTime(2014, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new DataPredicate.Builder()
                    .ParentId("Si")
                    .NewId("Si_15")
                    .AccumulationPeriod(new TimeSpan(0,15,0))
                    .From(new DateTime(2014, 1, 1))
                    .To(new DateTime(2017, 1, 1))
                    .Build(),
                new IndicatorPredicate.Builder()
                    .NewId("MA")
                    .Indicator(typeof(MA))
                    .DataPredicate(new DataPredicate.Builder()
                        .NewId("Test")
                        .ParentId("Si")
                        .AccumulationPeriod(new TimeSpan(0,0,5))
                        .From(new DateTime(2014, 1, 1))
                        .To(new DateTime(2017, 1, 1))
                        .Build())
                    .Build()
            });
        }

        public void Execution(CancellationToken token)
        {
            System.Diagnostics.Debug.WriteLine("Execution");
        }

        public void After(CancellationToken token)
        {
            System.Diagnostics.Debug.WriteLine("After");
        }
    }
}
