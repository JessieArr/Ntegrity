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
		public readonly List<ConstructorData> ConstructorData = new List<ConstructorData>();
		public readonly List<AttributeData> AttributeData = new List<AttributeData>();

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

		public override string ToString()
		{
			return ToString("");
		}

		public string ToString(string prefix)
		{
			var returnString = "";

			foreach (var attribute in AttributeData)
			{
				returnString += attribute + Environment.NewLine;
			}
			returnString += AccessLevelEnumHelpers.GetKeywordFromEnum(AccessLevel) + " ";

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
			returnString += Name;

			return returnString;
		}
	}
}