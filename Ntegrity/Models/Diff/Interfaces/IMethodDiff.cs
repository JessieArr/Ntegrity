using System.Collections.Generic;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models.Diff.Interfaces
{
    public interface IMethodDiff
    {
        bool HasChanged { get; }
        string MethodSignature { get; }

        List<IAttributeData> AddedAttributes { get; }
        List<IAttributeData> RemovedAttributes { get; }
    }
}