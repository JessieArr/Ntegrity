using System;

namespace Ntegrity.Models.Reflection
{
    internal class AttributeWrapper : IAttributeWrapper
    {
        private string _AttributeString; 
        public AttributeWrapper(Attribute attribute)
        {
            _AttributeString = attribute.ToString();
        }

        public override string ToString()
        {
            return _AttributeString;
        }
    }
}