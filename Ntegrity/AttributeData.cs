using System;

namespace Ntegrity
{
	public class AttributeData
	{
		public readonly string Name;

		public AttributeData(Attribute attribute)
		{
			Name = attribute.ToString();
		}
	}
}