using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using TradePlatform.Commons.Sistem;
using TradePlatform.SandboxApi.Presenters;

namespace TradePlatform.SandboxApi.Services
{
    public class SandboxDllProvider : ISandboxDllProvider
    {
        private string SandboxFolder => ".\\SANDBOXES";
        private readonly IFileManager _fileManager;

        public SandboxDllProvider()
        {
            _fileManager = ContainerBuilder.Container.Resolve<IFileManager>();
        }

        public IList<ISandboxPresenter> Get()
        {
            if (!_fileManager.IsDirectoryExist(SandboxFolder))
            {
                throw new Exception("Directory " + SandboxFolder + " - is not exist.");
            }

            DirectoryInfo info = new DirectoryInfo(SandboxFolder);
            IList<ISandboxPresenter> presenters = new List<ISandboxPresenter>();

            foreach (FileInfo file in info.GetFiles("*.dll"))
            {
                try
                {
                    var name = AssemblyName.GetAssemblyName(file.FullName);
                    Assembly.Load(name)
                        .GetTypes()
                        .Where(t => t != typeof(ISandbox) && typeof(ISandbox).IsAssignableFrom(t))
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
                catch (Exception ex) { }
            }

            return presenters;
        }
    }
}
