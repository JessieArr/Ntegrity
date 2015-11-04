using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ntegrity.Models.Reflection
{
    public class FieldInfoWrapper : IFieldInfoWrapper
    {
        public FieldInfoWrapper(FieldInfo fieldInfo)
        {
            FieldInfo = fieldInfo;

            IsPrivate = FieldInfo.IsPrivate;
            IsFamily = FieldInfo.IsFamily;
            IsAssembly = FieldInfo.IsAssembly;
            IsPublic = FieldInfo.IsPublic;
            IsSpecialName = FieldInfo.IsSpecialName;
        }

        public FieldInfo FieldInfo { get; }
        public bool IsPrivate { get; }
        public bool IsFamily { get; }
        public bool IsAssembly { get; }
        public bool IsPublic { get; }
        public bool IsSpecialName { get; }

        public IEnumerable<IAttributeWrapper> GetCustomAttributes()
        {
            return FieldInfo.GetCustomAttributes().Select(x => new AttributeWrapper(x));
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return FieldInfo.GetCustomAttributes(attributeType, inherit);
        }
    }
}