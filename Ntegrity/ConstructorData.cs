using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity
{
	public class ConstructorData
	{
		public readonly string ConstructorSignature;
		public readonly AccessLevelEnum AccessLevel;
		public readonly List<AttributeData> AttributeData = new List<AttributeData>();

		public ConstructorData(ConstructorInfo constructor)
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
	}
}