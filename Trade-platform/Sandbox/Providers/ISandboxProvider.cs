using System;
using System.Collections.Generic;
using TradePlatform.Sandbox.Presenters;

namespace TradePlatform.Sandbox.Providers
{
    public interface ISandboxProvider
    {
        IEnumerable<ISandboxPresenter> Get();
        ISandbox CreateInstance(Type type);
    }
}
