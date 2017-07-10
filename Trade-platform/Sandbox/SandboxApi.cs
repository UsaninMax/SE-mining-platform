using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Castle.Core;
using Microsoft.Practices.Unity;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding;
using TradePlatform.Sandbox.DataProviding.Predicates;
using TradePlatform.Sandbox.Models;

namespace TradePlatform.Sandbox
{
    public abstract class SandboxApi : ISandbox
    {
        public IList<Pair<DateTime, IEnumerable<IData>>> Data => _data;
        public CancellationToken Token => _token;
        public ICollection<IBot> Bots => _bots;
        private IList<Pair<DateTime, IEnumerable<IData>>> _data;
        private CancellationToken _token;
        private ICollection<IBot> _bots;

        public void SetToken(CancellationToken token)
        {
            _token = token;
        }

        public void SetUpBots(ICollection<IBot> bots)
        {
            _bots = bots;
        }

        public void BuildData()
        {
            if (_token.IsCancellationRequested) { return; }
            IDataProvider dataProvider = ContainerBuilder.Container.Resolve<IDataProvider>();
            _data = dataProvider.Get(SetUpData(), _token);
        }

        protected void Execute()
        {
            if (_token.IsCancellationRequested) { return; }
            var continuation = Task.WhenAll(_bots?.Select(x =>
            {
                return Task.Run(() =>
                {
                    x.SetUpData(Data);
                    x.Execute();
                }, _token);

            }).ToList());
            continuation.Wait();
            if (continuation.Status != TaskStatus.RanToCompletion)
            {
                throw new Exception("Bot during execution time throw exception");
            }
        }

        public void CleanMemory()
        {
            _data = null;
            _bots = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public abstract ICollection<IPredicate> SetUpData();
        public abstract void Execution();
        public abstract void AfterExecution();
    }
}
