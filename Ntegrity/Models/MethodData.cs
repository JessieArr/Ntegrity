using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ntegrity.Models.Interfaces;
using Ntegrity.Models.Reflection;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models
{
    public class MethodData : IMethodData
    {
        public string MethodSignature { get; }
        public string ReturnType { get; }
        public bool IsInherited { get; }
        public string DeclaringType { get; }
        public AccessLevelEnum AccessLevel { get; }
        public List<IAttributeData> AttributeData { get; }

        public MethodData(IMethodInfoWrapper methodInfo)
        {
            AttributeData = new List<IAttributeData>();

            var methodString = methodInfo.ToString();
            var splitIndex = methodString.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
            ReturnType = methodString.Substring(0, splitIndex);
            MethodSignature = methodString.Substring(splitIndex + 1, methodString.Length - (splitIndex + 1));

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

            if (methodInfo.ReflectedType == null || methodInfo.DeclaringType == null)
            {
                throw new Exception("Method Lacked Declaring or Reflected Type Data.");
            }
            if (methodInfo.DeclaringType.FullName != methodInfo.ReflectedType.FullName)
            {
                IsInherited = true;
                DeclaringType = methodInfo.DeclaringType.Namespace + methodInfo.DeclaringType.Name;
            }

            var attributes = methodInfo.GetCustomAttributes();
            foreach (var attribute in attributes)
            {
                AttributeData.Add(new AttributeData(attribute));
            }
            AttributeData = AttributeData.OrderBy(x => x.Name).ToList();
        }

        public MethodData(string methodInfo)
        {
            AttributeData = new List<IAttributeData>();

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

        public new string ToString()
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

            returnString += outputSettings.MemberPrefix + AccessLevel.GetKeywordFromEnum()
                            + " " + ReturnType + " ";
            if (IsInherited)
            {
                returnString += DeclaringType + ".";
            }
            returnString += MethodSignature;
            return returnString;
        }
    }

    public interface IMethodData
    {
        string MethodSignature { get; }
        string ReturnType { get; }
        bool IsInherited { get; }
        string DeclaringType { get; }
        AccessLevelEnum AccessLevel { get; }
        List<IAttributeData> AttributeData { get; }

        string ToString();
        string ToString(NtegrityOutputSettings outputSettings);
    }
}