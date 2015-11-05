using System.Collections.Generic;

namespace Ntegrity.Models.Diff.Interfaces
{
    public interface IFieldDiff
    {
        bool HasChanged { get; }
        string FieldSignature { get; }

        List<AttributeData> AddedAttributes { get; }
        List<AttributeData> RemovedAttributes { get; }
    }
}