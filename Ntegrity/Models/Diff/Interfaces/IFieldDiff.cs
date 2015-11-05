using System.Collections.Generic;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff.Interfaces
{
    public interface IFieldDiff
    {
        bool HasChanged { get; }
        string FieldSignature { get; }

        List<IAttributeData> AddedAttributes { get; }
        List<IAttributeData> RemovedAttributes { get; }
    }
}