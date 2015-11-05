using System.Collections.Generic;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff.Interfaces
{
    public interface IPropertyDiff
    {
        bool HasChanged { get; }
        string PropertySignature { get; }

        List<IAttributeData> AddedAttributes { get; }
        List<IAttributeData> RemovedAttributes { get; }
    }
}