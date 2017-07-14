using TradePlatform.Sandbox.Models;
using TradePlatform.Sandbox.Transactios.Models;

namespace TradePlatform.Sandbox.Transactios
{
    public interface ITransactionBuilder
    {
        Transaction Build(OpenPositionRequest request, Tick tick);
    }
}
