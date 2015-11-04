using System.Collections;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Ntegrity.Models.Reflection
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