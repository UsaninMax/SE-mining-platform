using SEMining.Sandbox.Models;
using SEMining.Sandbox.Transactios.Models;

namespace SEMining.Sandbox.Transactios
{
    public interface ITransactionBuilder
    {
        Transaction Build(OpenPositionRequest request, Tick tick);
        void Reset();
    }
}
