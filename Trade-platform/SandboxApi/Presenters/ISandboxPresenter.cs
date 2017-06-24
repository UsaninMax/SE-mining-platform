namespace TradePlatform.SandboxApi.Presenters
{
    public interface ISandboxPresenter
    {
        void Execute();
        void StopExecution();
        bool IsActive();
    }
}
