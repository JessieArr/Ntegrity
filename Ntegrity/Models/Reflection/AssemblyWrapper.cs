using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models.Reflection
{
    public class AssemblyWrapper : IAssemblyWrapper
    {
        public AssemblyWrapper(Assembly assemblyToAnalyze)
        {
            Assembly = assemblyToAnalyze;

            ImageRuntimeVersion = Assembly.ImageRuntimeVersion;
        }

        public Assembly Assembly { get; }
        public string ImageRuntimeVersion { get; }
        public IAssemblyNameWrapper GetName()
        {
            return new AssemblyNameWrapper(Assembly.GetName());
        }

        public IAssemblyNameWrapper[] GetReferencedAssemblies()
        {
            return Assembly.GetReferencedAssemblies().Select(x => new AssemblyNameWrapper(x)).ToArray();
        }

        public ITypeWrapper[] GetTypes()
        {
            return Assembly.GetTypes().Select(x => new TypeWrapper(x)).ToArray();
        }
    }
}
