using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity
{
    public class PropertyData
    {
        public readonly string PropertySignature;
        public readonly AccessLevelEnum GetterAccessLevel;
        public readonly bool HasGetter;
        public readonly AccessLevelEnum SetterAccessLevel;
        public readonly bool HasSetter;
        public readonly List<AttributeData> AttributeData = new List<AttributeData>();

        public PropertyData(PropertyInfo propertyInfo)
        {
            PropertySignature = propertyInfo.ToString();

            var getter = propertyInfo.GetGetMethod();
            HasGetter = getter != null;
            if (HasGetter)
            {
                if (getter.IsPrivate)
                {
                    GetterAccessLevel = AccessLevelEnum.Private;
                }
                if (getter.IsFamily)
                {
                    GetterAccessLevel = AccessLevelEnum.Protected;
                }
                if (getter.IsAssembly)
                {
                    GetterAccessLevel = AccessLevelEnum.Internal;
                }
                if (getter.IsPublic)
                {
                    GetterAccessLevel = AccessLevelEnum.Public;
                }
            }

            var setter = propertyInfo.GetSetMethod();
            HasSetter = setter != null;
            if (HasSetter)
            {
                if (setter.IsPrivate)
                {
                    SetterAccessLevel = AccessLevelEnum.Private;
                }
                if (setter.IsFamily)
                {
                    SetterAccessLevel = AccessLevelEnum.Protected;
                }
                if (setter.IsAssembly)
                {
                    SetterAccessLevel = AccessLevelEnum.Internal;
                }
                if (setter.IsPublic)
                {
                    SetterAccessLevel = AccessLevelEnum.Public;
                }
            }

            var attributes = propertyInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                AttributeData.Add(new AttributeData(attribute));
            }
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string prefix)
        {
            var accessorString = "{ ";
            if (HasGetter)
            {
                accessorString += GetterAccessLevel.GetKeywordFromEnum() + " get; ";
            }
            if (HasSetter)
            {
                accessorString += SetterAccessLevel.GetKeywordFromEnum() + " set; ";
            }
            accessorString += " }";

            return prefix + PropertySignature + " " + accessorString;
        }
    }
}