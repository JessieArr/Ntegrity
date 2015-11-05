namespace Ntegrity.Models.Interfaces
{
    public interface IAttributeData
    {
        string Name { get; }
        bool IsCompilerGenerated { get; }
        string ToString();
    }
}