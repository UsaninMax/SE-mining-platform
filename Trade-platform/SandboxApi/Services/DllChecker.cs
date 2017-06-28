using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TradePlatform.SandboxApi.Services
{
    [Serializable]
    public class DllChecker
    {
        public ICollection<string> GetSuitable(string rootFolder)
        {
            DirectoryInfo info = new DirectoryInfo(rootFolder);
            ICollection<string> patches = new HashSet<string>();

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
                            patches.Add(file.FullName);
                        });
                }
                catch (Exception ex) { }
            }

            return patches;
        }
    }
}
