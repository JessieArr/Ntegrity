using System.Collections.Generic;

namespace Ntegrity.Models.Interfaces
{
    public interface IConstructorData
    {
        string ConstructorSignature { get; }
        AccessLevelEnum AccessLevel { get; }
        List<IAttributeData> AttributeData { get; }
    }
}