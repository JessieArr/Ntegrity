using System.Reflection;

namespace Ntegrity.Models.Reflection
{
    public class AssemblyNameWrapper : IAssemblyNameWrapper
    {
        public AssemblyNameWrapper(AssemblyName assemblyName)
        {
            Name = assemblyName.Name;
            FullName = assemblyName.FullName;
            Version = assemblyName.Version.ToString();
        }

        public string Name { get; }
        public string FullName { get; }
        public string Version { get; }
    }
}