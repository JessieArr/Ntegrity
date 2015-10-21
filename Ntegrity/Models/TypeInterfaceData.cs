using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ntegrity.Models
{
	public class TypeInterfaceData
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

        public TypeInterfaceData(Type typeToAnalyze)
		{
			Name = typeToAnalyze.FullName;

			var foundType = false;
			if (typeToAnalyze.IsClass)
			{
				Type = TypeEnum.Class;
				foundType = true;
			}
			if (typeToAnalyze.IsInterface)
			{
				Type = TypeEnum.Interface;
				foundType = true;
			}
			if (typeToAnalyze.IsEnum)
			{
				Type = TypeEnum.Enum;
				foundType = true;
			}
			else
			{
				// Structs are value types, but not enums. Enums are both.
				if (typeToAnalyze.IsValueType)
				{
					Type = TypeEnum.Struct;
					foundType = true;
				}
			}
			if (!foundType)
			{
				throw new NtegrityException("Unable to determine data type for type: " + typeToAnalyze.AssemblyQualifiedName);
			}

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
			CollectConstructorData(typeToAnalyze);
            CollectMethodData(typeToAnalyze);
            CollectPropertyData(typeToAnalyze);
            CollectFieldData(typeToAnalyze);

            if (typeToAnalyze.BaseType != null 
                && typeToAnalyze.BaseType.FullName != "System.Object"
                && typeToAnalyze.BaseType.FullName != "System.ValueType"
                && typeToAnalyze.BaseType.FullName != "System.Enum")
            {
                InheritsFrom = typeToAnalyze.BaseType.FullName;
            }
            
            ImplementsInterfaces = typeToAnalyze.GetInterfaces().Select(x => x.FullName).ToList();
        }

	    public TypeInterfaceData(string typeString)
	    {
	        
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

            foreach (var attribute in AttributeData)
            {
                returnString += outputSettings.TypePrefix + attribute + Environment.NewLine;
            }
            returnString += outputSettings.TypePrefix + AccessLevelEnumHelpers.GetKeywordFromEnum(AccessLevel) + " ";

            if (Type == TypeEnum.Class)
            {
                if (IsStatic)
                {
                    returnString += "static ";
                }
                else
                {
                    if (IsAbstract)
                    {
                        returnString += "abstract ";
                    }
                    if (IsSealed)
                    {
                        returnString += "sealed ";
                    }
                }
            }

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

            if (MethodData.Count > 0)
            {
                returnString += outputSettings.TypePrefix + "METHODS:" + Environment.NewLine;
                foreach (var method in MethodData)
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