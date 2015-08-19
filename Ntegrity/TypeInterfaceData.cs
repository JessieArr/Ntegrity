using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;

namespace Ntegrity
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
			if (!typeToAnalyze.IsVisible && typeToAnalyze.IsNotPublic)
			{
				AccessLevel = AccessLevelEnum.Internal;
				foundAccessLevel = true;
			}
			if (typeToAnalyze.IsPublic)
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
            var constructors = typeToAnalyze.GetConstructors();

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
                MethodData.Add(new MethodData(method));
            }
        }

        private void CollectPropertyData(Type typeToAnalyze)
        {
            var properties = typeToAnalyze.GetProperties();

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
                FieldData.Add(new FieldData(field));
            }
        }

        public override string ToString()
		{
			return ToString("");
		}

		public string ToString(string prefix)
		{
			var returnString = "";

			foreach (var attribute in AttributeData)
			{
				returnString += prefix + attribute + Environment.NewLine;
			}
			returnString += prefix + AccessLevelEnumHelpers.GetKeywordFromEnum(AccessLevel) + " ";

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

		    if (ConstructorData.Count > 0)
		    {
                returnString += prefix + "CONSTRUCTORS:" + Environment.NewLine;
                foreach (var constructor in ConstructorData)
                {
                    returnString += prefix + constructor.ToString(prefix) + Environment.NewLine;
                }
            }

            if (MethodData.Count > 0)
            {
                returnString += prefix + "METHODS:" + Environment.NewLine;
                foreach (var method in MethodData)
                {
                    returnString += prefix + method.ToString(prefix) + Environment.NewLine;
                }
            }

            if (PropertyData.Count > 0)
            {
                returnString += prefix + "PROPERTIES:" + Environment.NewLine;
                foreach (var property in PropertyData)
                {
                    returnString += prefix + property.ToString(prefix) + Environment.NewLine;
                }
            }

            if (FieldData.Count > 0)
            {
                returnString += prefix + "FIELDS:" + Environment.NewLine;
                foreach (var field in FieldData)
                {
                    returnString += prefix + field.ToString(prefix) + Environment.NewLine;
                }
            }

            return returnString;
		}
	}
}