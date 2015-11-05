using System.Collections.Generic;

namespace Ntegrity.Models.Interfaces
{
    public interface IPropertyData
    {
        string PropertySignature { get; }
        AccessLevelEnum GetterAccessLevel { get; }
        bool HasGetter { get; }
        AccessLevelEnum SetterAccessLevel { get; }
        bool HasSetter { get; }
        List<IAttributeData> AttributeData { get; }
    }
}