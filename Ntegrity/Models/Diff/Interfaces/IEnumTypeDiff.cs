using System.Collections.Generic;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff.Interfaces
{
    public interface IEnumTypeDiff
    {
        bool HasChanged { get; }
        string Name { get; }

        List<IAttributeData> AddedAttributes { get; }
        List<IAttributeData> RemovedAttributes { get; }

        List<IFieldData> AddedFields { get; }
        List<IFieldData> RemovedFields { get; }

        List<IMethodData> AddedMethods { get; }
        List<IMethodData> RemovedMethods { get; }
    }
}