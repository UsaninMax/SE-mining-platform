using SE_mining_base.Sandbox.Models;
using SE_mining_base.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public interface ITransactionBuilder
    {
        Transaction Build(OpenPositionRequest request, Tick tick);
        void Reset();
    }
}
