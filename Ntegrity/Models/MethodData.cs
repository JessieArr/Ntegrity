using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity.Models
{
    public class MethodData
    {
        public readonly string MethodSignature;
        public readonly AccessLevelEnum AccessLevel;
        public readonly List<AttributeData> AttributeData = new List<AttributeData>();

        public MethodData(MethodInfo methodInfo)
        {
            MethodSignature = methodInfo.ToString();
            if (methodInfo.IsPrivate)
            {
                AccessLevel = AccessLevelEnum.Private;
            }
            if (methodInfo.IsFamily)
            {
                AccessLevel = AccessLevelEnum.Protected;
            }
            if (methodInfo.IsAssembly)
            {
                AccessLevel = AccessLevelEnum.Internal;
            }
            if (methodInfo.IsPublic)
            {
                AccessLevel = AccessLevelEnum.Public;
            }

            var attributes = methodInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                AttributeData.Add(new AttributeData(attribute));
            }
        }

        public MethodData(string methodInfo)
        {
            MethodSignature = methodInfo;
        }

        public override string ToString()
        {
            return ToString("");
        }

        public string ToString(string prefix)
        {
            return ToString(prefix, new NtegrityOutputSettings());
        }

        public string ToString(string prefix, NtegrityOutputSettings outputSettings)
        {
            var returnString = "";
            if (!AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                outputSettings.ShowTypesAtOrAboveAccessLevel))
            {
                return returnString;
            }

            returnString += prefix + AccessLevel.GetKeywordFromEnum() + " " + MethodSignature;
            return returnString;
        }
    }
}