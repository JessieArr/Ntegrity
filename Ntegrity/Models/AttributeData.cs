using System;
using Ntegrity.Models.Interfaces;
using Ntegrity.Models.Reflection;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models
{
	public class AttributeData : IAttributeData
    {
		public string Name { get; }
	    public bool IsCompilerGenerated { get; }

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