using System;
using Ntegrity.Models.Reflection;

namespace Ntegrity.Models
{
	public class AttributeData
	{
		public readonly string Name;
	    public bool IsCompilerGenerated;

        public AttributeData(IAttributeWrapper attribute)
        {
            Name = attribute.ToString();
            var attributeParts = Name.Split();
            var attributeTypeName = attributeParts[attributeParts.Length - 1];
            IsCompilerGenerated = attributeTypeName.StartsWith("__");
        }

        public AttributeData(string attributeString)
        {
            Name = attributeString;
            var attributeParts = Name.Split();
            var attributeTypeName = attributeParts[attributeParts.Length - 1];
            IsCompilerGenerated = attributeTypeName.StartsWith("__");
        }

        public override string ToString()
		{
			return "[" + Name + "]";
		}
	}
}