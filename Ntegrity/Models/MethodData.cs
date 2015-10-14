using System;
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

        public string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

        public string ToString(NtegrityOutputSettings outputSettings)
        {
            var returnString = "";
            if (!AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                outputSettings.ShowTypesAtOrAboveAccessLevel))
            {
                return returnString;
            }

            if (AttributeData.Count > 0)
            {
                foreach (var attribute in AttributeData)
                {
                    if (!outputSettings.ShowCompilerAttributes)
                    {
                        if (attribute.IsCompilerGenerated)
                        {
                            continue;
                        }
                    }

                    returnString += outputSettings.MemberPrefix + "[" + attribute.Name + "]" + Environment.NewLine;
                }
            }

            returnString += outputSettings.MemberPrefix + AccessLevel.GetKeywordFromEnum() + " " + MethodSignature;
            return returnString;
        }
    }
}