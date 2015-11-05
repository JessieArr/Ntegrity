using System.Collections.Generic;

namespace Ntegrity.Models.Interfaces
{
    public interface IFieldData
    {
        string FieldSignature { get; }
        AccessLevelEnum AccessLevel { get; }
        List<AttributeData> AttributeData { get; }
        string ToString();
        string ToString(NtegrityOutputSettings outputSettings);
    }
}