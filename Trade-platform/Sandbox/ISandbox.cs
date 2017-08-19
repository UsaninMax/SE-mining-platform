using System.Collections.Generic;
using System.Threading;
using TradePlatform.Charts.Data.Predicates.Basis;
using TradePlatform.Charts.Vizualization.Configurations;
using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.DataProviding.Predicates;

namespace TradePlatform.Sandbox
{
    public interface ISandbox
    {
        void SetToken(CancellationToken token);
        IEnumerable<IPredicate> SetUpData();
        void SetUpBots(IEnumerable<IBot> bots);
        void BuildData();
        void Execution();
        void AfterExecution();
        void CleanMemory();

        IEnumerable<PanelViewPredicate> SetUpCharts();
        void CreateCharts();
        void PopulateCharts(IEnumerable<ChartPredicate> predicates);
        void StoreCustomData(string key, IEnumerable<object> data);
        void CleanCustomeStorage();
    }
}
