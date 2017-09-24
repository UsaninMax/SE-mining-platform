using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using SEMining.Commons.Sistem;
using SEMining.Sandbox.Presenters;

namespace SEMining.Sandbox.Providers
{
    public class SandboxProvider : ISandboxProvider
    {
        private string _sandboxFolder => ".\\SANDBOXES";
        private readonly IFileManager _fileManager;

        public SandboxProvider()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
        }

        public IEnumerable<ISandboxPresenter> Get()
        {
            if (!_fileManager.IsDirectoryExist(_sandboxFolder))
            {
                _fileManager.CreateFolder(_sandboxFolder);
            }
            IList<ISandboxPresenter> presenters = new List<ISandboxPresenter>();
            AppDomain dom = AppDomain.CreateDomain("CheckerDomain");
            DllChecker checker = (DllChecker)dom.CreateInstanceAndUnwrap(typeof(DllChecker).Assembly.FullName, typeof(DllChecker).FullName);

            foreach (string file in checker.GetSuitableDll(_sandboxFolder))
            {
                var name = AssemblyName.GetAssemblyName(file);
                Assembly.Load(name)
                    .GetTypes()
                    .Where(t => typeof(SandboxAbstraction).IsAssignableFrom(t) && name.Name.Equals(t.Name))
                    .ToList()
                    .ForEach(x =>
                    {
                        SandboxAbstraction sandbox = (SandboxAbstraction)Activator.CreateInstance(x);
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
            return (SandboxAbstraction)Activator.CreateInstance(type);
        }
    }
}
