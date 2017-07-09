using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Sistem;
using TradePlatform.Sandbox.Presenters;

namespace TradePlatform.Sandbox.Providers
{
    public class SandboxProvider : ISandboxProvider
    {
        private string _sandboxFolder => ".\\SANDBOXES";
        private readonly IFileManager _fileManager;

        public SandboxProvider()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
        }

        public IList<ISandboxPresenter> Get()
        {
            if (!_fileManager.IsDirectoryExist(_sandboxFolder))
            {
                _fileManager.CreateFolder(_sandboxFolder);
            }
            IList<ISandboxPresenter> presenters = new List<ISandboxPresenter>();
            AppDomain dom = AppDomain.CreateDomain("CheckerDomain");
            DllChecker checker = (DllChecker)dom.CreateInstanceAndUnwrap(typeof(DllChecker).Assembly.FullName, typeof(DllChecker).FullName);

            foreach (string file in checker.GetSuitableDLL(_sandboxFolder))
            {
                Assembly.Load(AssemblyName.GetAssemblyName(file))
                    .GetTypes()
                    .Where(t => typeof(SandboxApi).IsAssignableFrom(t))
                    .ToList()
                    .ForEach(x =>
                    {
                        SandboxApi sandbox = (SandboxApi)Activator.CreateInstance(x);
                        var sandboxPresenter = ContainerBuilder.Container.Resolve<ISandboxPresenter>(
                            new DependencyOverride<ISandbox>(sandbox),
                            new DependencyOverride<string>(x.Name));
                        presenters.Add(sandboxPresenter);
                    });
            }
            return presenters;
        }

        public ISandbox CreateInstance(Type type)
        {
            return (SandboxApi)Activator.CreateInstance(type);
        }
    }
}
