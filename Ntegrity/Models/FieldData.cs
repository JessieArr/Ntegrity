using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity.Models
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

        public FieldData(string fieldString)
        {
            var sanitizedMethodInfo = fieldString.Replace("\t\t", "");
            var lines = sanitizedMethodInfo.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (var i = 0; i < lines.Length - 1; i++)
            {
                var attributeName = lines[i].Replace("[", "");
                attributeName = attributeName.Replace("]", "");
                AttributeData.Add(new AttributeData(attributeName));
            }

            var lastLine = lines[lines.Length - 1];
            FieldSignature = lastLine;
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

        public new string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

        public string ToString(NtegrityOutputSettings outputSettings)
        {
            if(!AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                outputSettings.ShowTypesAtOrAboveAccessLevel))
            {
                return "";
            }

            var returnString = "";

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

            return returnString + outputSettings.MemberPrefix + AccessLevel.GetKeywordFromEnum() + " " + FieldSignature;
        }
    }
}