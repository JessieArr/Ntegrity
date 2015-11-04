using System;
using System.Collections.Generic;
using System.Reflection;
using Ntegrity.Models.Reflection;

namespace Ntegrity.Models
{
	public class ConstructorData
	{
		public readonly string ConstructorSignature;
		public readonly AccessLevelEnum AccessLevel;
		public readonly List<AttributeData> AttributeData = new List<AttributeData>();

		public ConstructorData(IConstructorInfoWrapper constructor)
		{
			ConstructorSignature = constructor.ToString();
			if (constructor.IsPrivate)
			{
				AccessLevel = AccessLevelEnum.Private;
			}
			if (constructor.IsFamily)
			{
				AccessLevel = AccessLevelEnum.Protected;
			}
			if (constructor.IsAssembly)
			{
				AccessLevel = AccessLevelEnum.Internal;
			}
			if (constructor.IsPublic)
			{
				AccessLevel = AccessLevelEnum.Public;
			}

			var attributes = constructor.GetCustomAttributes();
			foreach (var attribute in attributes)
			{
				AttributeData.Add(new AttributeData(attribute));
			}
		}

	    public ConstructorData(string constructorString)
	    {
            var sanitizedMethodInfo = constructorString.Replace("\t\t", "");
            var lines = sanitizedMethodInfo.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            for (var i = 0; i < lines.Length - 1; i++)
            {
                var attributeName = lines[i].Replace("[", "");
                attributeName = attributeName.Replace("]", "");
                AttributeData.Add(new AttributeData(attributeName));
            }

            var lastLine = lines[lines.Length - 1];
            ConstructorSignature = lastLine;
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

        public override string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

        public string ToString(NtegrityOutputSettings outputSettings)
        {
            if (!AccessLevel.HasAvailabilityEqualToOrGreaterThan(
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

            return returnString + outputSettings.MemberPrefix + AccessLevel.GetKeywordFromEnum() + " " + ConstructorSignature;
        }
    }
}