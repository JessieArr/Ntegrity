using System.Reflection;

namespace Ntegrity.Models.Reflection.Interfaces
{
    public interface IAssemblyWrapper
    {
        Assembly Assembly { get; }
        string ImageRuntimeVersion { get; }
        IAssemblyNameWrapper GetName();
        IAssemblyNameWrapper[] GetReferencedAssemblies();
        ITypeWrapper[] GetTypes();
    }
}