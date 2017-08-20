using System.Collections.Generic;
using TradePlatform.SandboxApi.Presenters;

namespace TradePlatform.SandboxApi.Services
{
    public interface ISandboxDllProvider
    {
        IList<ISandboxPresenter> Get();
    }
}
