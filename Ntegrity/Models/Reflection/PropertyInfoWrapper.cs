using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models.Reflection
{
    public class PropertyInfoWrapper : IPropertyInfoWrapper
    {
        public PropertyInfoWrapper(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }

        public PropertyInfo PropertyInfo { get; }
        public IMethodInfoWrapper GetGetMethod(bool nonPublic)
        {
            var getMethod = PropertyInfo.GetGetMethod(nonPublic);
            if (getMethod == null)
            {
                return null;
            }
            return new MethodInfoWrapper(getMethod);
        }

        public IMethodInfoWrapper GetSetMethod(bool nonPublic)
        {
            var setMethod = PropertyInfo.GetSetMethod(nonPublic);
            if (setMethod == null)
            {
                return null;
            }
            return new MethodInfoWrapper(setMethod);
        }

        public IEnumerable<IAttributeWrapper> GetCustomAttributes()
        {
            return PropertyInfo.GetCustomAttributes().Select(x => new AttributeWrapper(x));
        }

        public override string ToString()
        {
            return PropertyInfo.ToString();
        }
    }
}