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
            var sanitizedMethodInfo = methodInfo.Replace("\t\t", "");
            var lines = sanitizedMethodInfo.Split(new[] {Environment.NewLine}, StringSplitOptions.None);

            for (var i = 0; i < lines.Length - 1; i++)
            {
                var attributeName = lines[i].Replace("[", "");
                attributeName = attributeName.Replace("]", "");
                AttributeData.Add(new AttributeData(attributeName));
            }

            var lastLine = lines[lines.Length - 1];
            MethodSignature = lastLine;
            var lastLineParts = lastLine.Split(' ');
            var accessLevel = lastLineParts[0];

            switch (accessLevel)
            {
                case "public":
                    AccessLevel = AccessLevelEnum.Public;
                    break;
                case "private":
                    AccessLevel = AccessLevelEnum.Private;
                    break;
                case "internal":
                    AccessLevel = AccessLevelEnum.Internal;
                    break;
                case "protected":
                    AccessLevel = AccessLevelEnum.Protected;
                    break;
            }
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