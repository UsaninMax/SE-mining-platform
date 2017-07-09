using TradePlatform.Sandbox.Bots;
using TradePlatform.Sandbox.Models;

namespace TestSandboxModule
{
    public class TestBot : BotApi
    {
        public override void Execution(Slice slice)
        {
            System.Diagnostics.Debug.WriteLine("Bot name = " + GetId() + " -receive slice " + slice);
        }

        public override int Score()
        {
            return 100;
        }
    }
}
