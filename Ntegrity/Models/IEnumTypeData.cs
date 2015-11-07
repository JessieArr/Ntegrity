using System.Collections.Generic;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Models
{
    public interface IEnumTypeData
    {
        string Name { get; }
        TypeEnum Type { get; }
        AccessLevelEnum AccessLevel { get; }

        List<IAttributeData> AttributeData { get; }
        List<IMethodData> MethodData { get; }
        List<IFieldData> FieldData { get; }
        List<string> ImplementsInterfaces { get; }

        string ToString();
        string ToString(NtegrityOutputSettings outputSettings);
    }
}