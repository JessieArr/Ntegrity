using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ntegrity.Models.Reflection.Interfaces;

namespace Ntegrity.Models.Reflection
{
    public class ConstructorInfoWrapper : IConstructorInfoWrapper
    {
        public ConstructorInfoWrapper(ConstructorInfo constructorInfo)
        {
            ConstructorInfo = constructorInfo;

            IsPrivate = constructorInfo.IsPrivate;
            IsFamily = constructorInfo.IsFamily;
            IsAssembly = constructorInfo.IsAssembly;
            IsPublic = constructorInfo.IsPublic;
        }

        public ConstructorInfo ConstructorInfo { get; }
        public bool IsPrivate { get; }
        public bool IsFamily { get; }
        public bool IsAssembly { get; }
        public bool IsPublic { get; }

        public IEnumerable<IAttributeWrapper> GetCustomAttributes()
        {
            return ConstructorInfo.GetCustomAttributes().Select(x => new AttributeWrapper(x)).ToArray();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return ConstructorInfo.GetCustomAttributes(attributeType, inherit).Select(x => new AttributeWrapper((Attribute)x)).ToArray();
        }

        public override string ToString()
        {
            return ConstructorInfo.ToString();
        }
    }
}