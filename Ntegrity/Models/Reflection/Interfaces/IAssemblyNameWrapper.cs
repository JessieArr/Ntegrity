namespace Ntegrity.Models.Reflection.Interfaces
{
    public interface IAssemblyNameWrapper
    {
        string Name { get; }
        string FullName { get; }
        string Version { get; }
    }
}