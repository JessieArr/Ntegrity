using System.Collections.Generic;

namespace Ntegrity.Models.Interfaces
{
    public interface IFieldData
    {
        string FieldSignature { get; }
        AccessLevelEnum AccessLevel { get; }
        List<IAttributeData> AttributeData { get; }
        string ToString();
        string ToString(NtegrityOutputSettings outputSettings);
    }
}