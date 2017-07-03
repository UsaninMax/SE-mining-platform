using TradePlatform.SandboxApi.Bots;
using TradePlatform.SandboxApi.Models;

namespace TestSandboxModule
{
    public class TestBot : Bot
    {
        public override void Execution(Slice slice)
        {
            System.Diagnostics.Debug.WriteLine("Bot name = " + Id + " -receive slice " + slice);
        }

        public override int Score()
        {
            return 100;
        }
    }
}
