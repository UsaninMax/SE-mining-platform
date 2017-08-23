using System.Collections.Generic;
using System.Threading;
using SEMining.Sandbox.Bots;
using SE_mining_base.Charts.Data.Predicates.Basis;
using SE_mining_base.Charts.Vizualization.Configurations;
using SE_mining_base.Sandbox.DataProviding.Predicates;

namespace SEMining.Sandbox
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
