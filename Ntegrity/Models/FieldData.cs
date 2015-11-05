using System;
using System.Collections.Generic;
using System.Reflection;
using Ntegrity.Models.Interfaces;
using Ntegrity.Models.Reflection;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models
{
    public class FieldData : IFieldData
    {
        public string FieldSignature { get; }
        public AccessLevelEnum AccessLevel { get; }
        public List<AttributeData> AttributeData { get; }

        public FieldData(IFieldInfoWrapper fieldInfo)
        {
            AttributeData = new List<AttributeData>();

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
            AttributeData = new List<AttributeData>();
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