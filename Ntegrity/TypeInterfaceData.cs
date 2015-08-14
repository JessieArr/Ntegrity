using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;

namespace Ntegrity
{
	public class TypeInterfaceData
	{
		public readonly string Name;
		public readonly TypeEnum Type;
		public readonly bool IsSealed;
		public readonly bool IsAbstract;
		public readonly List<ConstructorData> ConstructorData = new List<ConstructorData>();

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

			IsSealed = typeToAnalyze.IsSealed;
			IsAbstract = typeToAnalyze.IsAbstract;

			CollectConstructorData(typeToAnalyze);
		}

		private void CollectConstructorData(Type typeToAnalyze)
		{
			var constructors = typeToAnalyze.GetConstructors();

			foreach (var constructor in constructors)
			{
				ConstructorData.Add(new ConstructorData(constructor));
			}
		}
	}
}