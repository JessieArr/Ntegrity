using System.Collections.Generic;

namespace Ntegrity.Models.Diff.Interfaces
{
    public interface IEnumTypeDiff
    {
        bool HasChanged { get; }
        string Name { get; }

        List<AttributeData> AddedAttributes { get; }
        List<AttributeData> RemovedAttributes { get; }

        List<FieldData> AddedFields { get; }
        List<FieldData> RemovedFields { get; }

        List<MethodData> AddedMethods { get; }
        List<MethodData> RemovedMethods { get; }
    }
}