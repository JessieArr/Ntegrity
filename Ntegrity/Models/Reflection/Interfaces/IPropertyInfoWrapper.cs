using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity.Models.Reflection.Interfaces
{
    public interface IPropertyInfoWrapper
    {
        PropertyInfo PropertyInfo { get; }

        IMethodInfoWrapper GetGetMethod(bool nonPublic);
        IMethodInfoWrapper GetSetMethod(bool nonPublic);

        IEnumerable<IAttributeWrapper> GetCustomAttributes();
    }
}