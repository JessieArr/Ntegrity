using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ntegrity.Models.Reflection;

namespace Ntegrity.Models
{
	public class EnumTypeData
	{
		public readonly string Name;
		public readonly TypeEnum Type;
		public readonly AccessLevelEnum AccessLevel;
		
		public readonly List<AttributeData> AttributeData = new List<AttributeData>();
        public readonly List<MethodData> MethodData = new List<MethodData>();
        public readonly List<FieldData> FieldData = new List<FieldData>();
        public readonly List<string> ImplementsInterfaces;

        public EnumTypeData(ITypeWrapper typeToAnalyze)
		{
            if (!typeToAnalyze.IsEnum)
			{
                throw new NtegrityException("Type: " + typeToAnalyze.AssemblyQualifiedName + " is not an Enum.");
            }

            Name = typeToAnalyze.FullName;

            Type = TypeEnum.Enum;

            var foundAccessLevel = false;
            if (typeToAnalyze.IsNestedPrivate)
            {
                AccessLevel = AccessLevelEnum.Private;
                foundAccessLevel = true;
            }
            if (!typeToAnalyze.IsVisible && typeToAnalyze.IsNotPublic 
                || typeToAnalyze.IsNestedAssembly)
			{
				AccessLevel = AccessLevelEnum.Internal;
				foundAccessLevel = true;
			}
			if (typeToAnalyze.IsPublic || typeToAnalyze.IsNestedPublic)
			{
				AccessLevel = AccessLevelEnum.Public;
				foundAccessLevel = true;
			}
			if (typeToAnalyze.IsNestedFamily)
			{
				AccessLevel = AccessLevelEnum.Protected;
				foundAccessLevel = true;
			}
			if (!foundAccessLevel)
			{
				throw new NtegrityException("Unable to determine access level for type: " + typeToAnalyze.AssemblyQualifiedName);
			}

			CollectAttributeData(typeToAnalyze);
            AttributeData = AttributeData.OrderBy(x => x.Name).ToList();

            CollectMethodData(typeToAnalyze);
            MethodData = MethodData.OrderBy(x => x.MethodSignature).ToList();

            CollectFieldData(typeToAnalyze);
            FieldData = FieldData.OrderBy(x => x.FieldSignature).ToList();
            
            ImplementsInterfaces = typeToAnalyze.GetInterfaces().Select(x => x.FullName).ToList();
        }

	    public EnumTypeData(string typeString)
	    {
            var sanitizedTypeInfo = typeString.Replace("\t", "");
            var lines = sanitizedTypeInfo.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

	        var i = 0;
            while(lines[i].StartsWith("[") && i < 1000)
            {
                var attributeName = lines[i].Replace("[", "");
                attributeName = attributeName.Replace("]", "");
                AttributeData.Add(new AttributeData(attributeName));
                i++;
            }

            var enumNameLine = lines[i];
            var enumNameLineParts = enumNameLine.Split(' ');
            var accessLevel = enumNameLineParts[0];
            var type = enumNameLineParts[1];

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

	        if (!type.Equals("enum"))
	        {
	            throw new Exception("Non-enum type passed to EnumTypeData constructor!");
	        }
            Type = TypeEnum.Enum;
        }

        private void CollectAttributeData(ITypeWrapper typeToAnalyze)
		{
			var attributes = typeToAnalyze.GetCustomAttributes(true);

			foreach (var attribute in attributes)
			{
                var wrappedAttribute = new AttributeWrapper((Attribute)attribute);
				AttributeData.Add(new AttributeData(wrappedAttribute));
			}
		}

        private void CollectMethodData(ITypeWrapper typeToAnalyze)
        {
            var methods = typeToAnalyze.GetMethods();

            foreach (var method in methods)
            {
                if (method.IsSpecialName ||
                    method.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
                {
                    continue;
                }
                MethodData.Add(new MethodData(method));
            }
        }

        private void CollectFieldData(ITypeWrapper typeToAnalyze)
        {
            var fields = typeToAnalyze.GetFields();

            foreach (var field in fields)
            {
                if (field.IsSpecialName ||
                    field.GetCustomAttributes(typeof(CompilerGeneratedAttribute), true).Any())
                {
                    continue;
                }
                FieldData.Add(new FieldData(field));
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

            var attributes = AttributeData.OrderBy(x => x.Name);
            foreach (var attribute in attributes)
            {
                returnString += outputSettings.TypePrefix + attribute + Environment.NewLine;
            }
            returnString += outputSettings.TypePrefix + AccessLevelEnumHelpers.GetKeywordFromEnum(AccessLevel) + " ";

            returnString += TypeEnumHelpers.GetKeywordFromEnum(Type) + " ";
            returnString += Name + Environment.NewLine;

            if (ImplementsInterfaces.Count > 0)
            {
                returnString += outputSettings.TypePrefix + "IMPLEMENTS:" + Environment.NewLine;
                foreach (var interfaceName in ImplementsInterfaces)
                {
                    returnString += outputSettings.MemberPrefix + interfaceName + Environment.NewLine;
                }
            }

            var methodsToShow = MethodData.Where(
                (x) =>
                {
                    if (!outputSettings.ShowInheritedMethods && x.IsInherited)
                    {
                        return false;
                    }
                    if (!outputSettings.ShowMethodsInheritedFromSystemTypes
                    && x.IsInherited
                    && x.DeclaringType.StartsWith("System."))
                    {
                        return false;
                    }
                    return true;
                });
            if (methodsToShow.Any())
            {
                returnString += outputSettings.TypePrefix + "METHODS:" + Environment.NewLine;
                foreach (var method in methodsToShow)
                {
                    if (!method.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    outputSettings.ShowTypesAtOrAboveAccessLevel))
                    {
                        continue;
                    }
                    returnString += method.ToString(outputSettings) + Environment.NewLine;
                }
            }

            if (FieldData.Count > 0)
            {
                returnString += outputSettings.TypePrefix + "FIELDS:" + Environment.NewLine;
                foreach (var field in FieldData)
                {
                    if (!field.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    outputSettings.ShowTypesAtOrAboveAccessLevel))
                    {
                        continue;
                    }
                    returnString += field.ToString(outputSettings) + Environment.NewLine;
                }
            }

            return returnString;
        }
    }
}