using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity
{
    public class FieldData
    {
        public readonly string FieldSignature;
        public readonly AccessLevelEnum AccessLevel;
        public readonly List<AttributeData> AttributeData = new List<AttributeData>();

        public FieldData(FieldInfo fieldInfo)
        {
            FieldSignature = fieldInfo.ToString();
            if (fieldInfo.IsPrivate)
            {
                AccessLevel = AccessLevelEnum.Private;
            }
            if (fieldInfo.IsFamily)
            {
                AccessLevel = AccessLevelEnum.Protected;
            }
            if (fieldInfo.IsAssembly)
            {
                AccessLevel = AccessLevelEnum.Internal;
            }
            if (fieldInfo.IsPublic)
            {
                AccessLevel = AccessLevelEnum.Public;
            }

            var attributes = fieldInfo.GetCustomAttributes();
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
            return prefix + AccessLevel.GetKeywordFromEnum() + " " + FieldSignature;
        }

        public string ToString(string prefix, NtegrityOutputSettings outputSettings)
        {
            if(AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                outputSettings.ShowTypesAtOrAboveAccessLevel))
            {
                return "";
            }
            return prefix + AccessLevel.GetKeywordFromEnum() + " " + FieldSignature;
        }
    }
}