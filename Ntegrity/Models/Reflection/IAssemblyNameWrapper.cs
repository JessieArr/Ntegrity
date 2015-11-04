namespace Ntegrity.Models.Reflection
{
    public interface IAssemblyNameWrapper
    {
        string Name { get; }
        string FullName { get; }
        string Version { get; }
    }
}