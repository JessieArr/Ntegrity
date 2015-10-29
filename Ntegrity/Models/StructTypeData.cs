using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ntegrity.Models
{
	public class StructTypeData
    {
		public readonly string Name;
		public readonly TypeEnum Type;
		public readonly AccessLevelEnum AccessLevel;
		public readonly bool IsSealed;
		public readonly bool IsAbstract;
		public readonly bool IsStatic;
		
		public readonly List<AttributeData> AttributeData = new List<AttributeData>();
        public readonly List<ConstructorData> ConstructorData = new List<ConstructorData>();
        public readonly List<MethodData> MethodData = new List<MethodData>();
        public readonly List<PropertyData> PropertyData = new List<PropertyData>();
        public readonly List<FieldData> FieldData = new List<FieldData>();
        public readonly string InheritsFrom;
        public readonly List<string> ImplementsInterfaces;

        public StructTypeData(Type typeToAnalyze)
		{
			Name = typeToAnalyze.FullName;

            if (!(typeToAnalyze.IsValueType && !typeToAnalyze.IsEnum))
            {
                throw new NtegrityException("Non-struct type passed to StructTypeData's constructor!");
            }
            Type = TypeEnum.Struct;

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

			IsSealed = typeToAnalyze.IsSealed;
			IsAbstract = typeToAnalyze.IsAbstract;
			// static types are both sealed and abstract. They can neither be inherited from nor instantiated.
			IsStatic = IsSealed && IsAbstract;

			CollectAttributeData(typeToAnalyze);
            AttributeData = AttributeData.OrderBy(x => x.ToString()).ToList();

            CollectConstructorData(typeToAnalyze);
            ConstructorData = ConstructorData.OrderBy(x => x.ToString()).ToList();

            CollectMethodData(typeToAnalyze);
            MethodData = MethodData.OrderBy(x => x.ToString()).ToList();

            CollectPropertyData(typeToAnalyze);
            PropertyData = PropertyData.OrderBy(x => x.ToString()).ToList();

            CollectFieldData(typeToAnalyze);
            FieldData = FieldData.OrderBy(x => x.ToString()).ToList();

            if (typeToAnalyze.BaseType != null 
                && typeToAnalyze.BaseType.FullName != "System.Object"
                && typeToAnalyze.BaseType.FullName != "System.ValueType"
                && typeToAnalyze.BaseType.FullName != "System.Enum")
            {
                InheritsFrom = typeToAnalyze.BaseType.FullName;
            }
            
            ImplementsInterfaces = typeToAnalyze.GetInterfaces().Select(x => x.FullName).ToList();
        }

	    public StructTypeData(string typeString)
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

            var structNameLine = lines[i];
            var structNameLineParts = structNameLine.Split(' ');
            var accessLevel = structNameLineParts[0];
            var type = structNameLineParts[1];

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

            if (!type.Equals("struct"))
            {
                throw new Exception("Non-enum type passed to EnumTypeData constructor!");
            }
            Type = TypeEnum.Struct;
        }

        private void CollectAttributeData(Type typeToAnalyze)
		{
			var attributes = typeToAnalyze.GetCustomAttributes(true);

			foreach (var attribute in attributes)
			{
				AttributeData.Add(new AttributeData((Attribute)attribute));
			}
		}

        private void CollectConstructorData(Type typeToAnalyze)
        {
            var constructors = typeToAnalyze.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var constructor in constructors)
            {
                ConstructorData.Add(new ConstructorData(constructor));
            }
        }

        private void CollectMethodData(Type typeToAnalyze)
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

        private void CollectPropertyData(Type typeToAnalyze)
        {
            var properties = typeToAnalyze.GetProperties(
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                PropertyData.Add(new PropertyData(property));
            }
        }

        private void CollectFieldData(Type typeToAnalyze)
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

            if (!String.IsNullOrEmpty(InheritsFrom))
            {
                returnString += outputSettings.TypePrefix + "INHERITS:" + Environment.NewLine;
                returnString += outputSettings.MemberPrefix + InheritsFrom + Environment.NewLine;
            }

            if (ImplementsInterfaces.Count > 0)
            {
                returnString += outputSettings.TypePrefix + "IMPLEMENTS:" + Environment.NewLine;
                foreach (var interfaceName in ImplementsInterfaces)
                {
                    returnString += outputSettings.MemberPrefix + interfaceName + Environment.NewLine;
                }
            }

            if (ConstructorData.Count > 0)
            {
                returnString += outputSettings.TypePrefix + "CONSTRUCTORS:" + Environment.NewLine;
                foreach (var constructor in ConstructorData)
                {
                    if (!constructor.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    outputSettings.ShowTypesAtOrAboveAccessLevel))
                    {
                        continue;
                    }
                    returnString += constructor.ToString(outputSettings) + Environment.NewLine;
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
                    returnString += method.ToString() + Environment.NewLine;
                }
            }

            if (PropertyData.Count > 0)
            {
                returnString += outputSettings.TypePrefix + "PROPERTIES:" + Environment.NewLine;
                foreach (var property in PropertyData)
                {
                    if (!(property.GetterAccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    outputSettings.ShowTypesAtOrAboveAccessLevel)
                    || property.SetterAccessLevel.HasAvailabilityEqualToOrGreaterThan((
                    outputSettings.ShowTypesAtOrAboveAccessLevel))))
                    {
                        continue;
                    }
                    returnString += property.ToString(outputSettings) + Environment.NewLine;
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