using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using SEMining.Commons.Loggers;

namespace SEMining.Sandbox.Providers
{
    [Serializable]
    public class DllChecker
    {
        public IEnumerable<string> GetSuitableDll(string rootFolder)
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
                        .Where(t => t != typeof(SandboxAbstraction) && typeof(SandboxAbstraction).IsAssignableFrom(t))
                        .ToList()
                        .ForEach(x =>
                        {
                            patches.Add(file.FullName);
                        });
                }
                catch (Exception ex)
                {
                    SystemLogger.Log.Error(ex);
                }
            }

            return patches;
        }
    }
}
