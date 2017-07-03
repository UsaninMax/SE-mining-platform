using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradePlatform.SandboxApi.DataProviding;
using TradePlatform.SandboxApi.DataProviding.Predicates;
using TradePlatform.SandboxApi.Models;
using Microsoft.Practices.Unity;
using TradePlatform.SandboxApi.Bots;

namespace TradePlatform.SandboxApi
{
    public abstract class Sandbox
    {
        private IList<IData> _data;
        public CancellationToken Token { get; set; }
        protected ICollection<Bot> _bots;
        public abstract ICollection<IPredicate> PrepareData();
        public abstract void Execution();
        public abstract void AfterExecution();

        public void BuildData()
        {
            if (Token.IsCancellationRequested) { return; }
            IDataProvider dataProvider = ContainerBuilder.Container.Resolve<IDataProvider>();
            _data = dataProvider.Get(PrepareData(), Token);
        }

        protected void Execute()
        {
            if (Token.IsCancellationRequested) { return; }
            var continuation = Task.WhenAll(_bots.Select(x =>
            {
                return Task.Run(() =>
                {
                    x.Data = _data;
                    x.Execute();
                }, Token);

            }).ToList());
            continuation.Wait();
            if (continuation.Status != TaskStatus.RanToCompletion)
            {
                throw new Exception("Bot during execution time throw exception");
            }
        }
    }
}
