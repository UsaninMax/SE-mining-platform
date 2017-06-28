using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Sistem;
using TradePlatform.SandboxApi.Presenters;

namespace TradePlatform.SandboxApi.Services
{
    public class SandboxDllProvider : ISandboxDllProvider
    {
        private string _sandboxFolder => ".\\SANDBOXES";
        private readonly IFileManager _fileManager;

        public SandboxDllProvider()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
        }

        public IList<ISandboxPresenter> Get()
        {
            if (!_fileManager.IsDirectoryExist(_sandboxFolder))
            {
                throw new Exception("Directory " + _sandboxFolder + " - is not exist.");
            }
            IList<ISandboxPresenter> presenters = new List<ISandboxPresenter>();
            AppDomain dom = AppDomain.CreateDomain("CheckerDomain");
            DllChecker checker = (DllChecker)dom.CreateInstanceAndUnwrap(typeof(DllChecker).Assembly.FullName, typeof(DllChecker).FullName);

            foreach (string file in checker.GetSuitable(_sandboxFolder))
            {
                Assembly.Load(AssemblyName.GetAssemblyName(file))
                    .GetTypes()
                    .Where(t => typeof(ISandbox).IsAssignableFrom(t))
                    .ToList()
                    .ForEach(x =>
                    {
                        ISandbox sandbox = (ISandbox)Activator.CreateInstance(x);
                        var sandboxPresenter = ContainerBuilder.Container.Resolve<ISandboxPresenter>(
                            new DependencyOverride<ISandbox>(sandbox),
                            new DependencyOverride<string>(x.Name));
                        presenters.Add(sandboxPresenter);
                    });
            }
            return presenters;
        }
    }
}
