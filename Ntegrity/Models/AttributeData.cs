using System;

namespace Ntegrity.Models
{
	public class AttributeData
	{
		public readonly string Name;

		public AttributeData(Attribute attribute)
		{
			Name = attribute.ToString();
		}

		public override string ToString()
		{
			return "[" + Name + "]";
		}
	}
}