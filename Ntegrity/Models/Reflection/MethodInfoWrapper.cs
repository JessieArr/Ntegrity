using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ntegrity.Models.Reflection
{
    public class MethodInfoWrapper : IMethodInfoWrapper
    {
        public MethodInfoWrapper(MethodInfo methodInfo)
        {
            MethodInfo = methodInfo;

            IsPrivate = methodInfo.IsPrivate;
            IsFamily = methodInfo.IsFamily;
            IsAssembly = methodInfo.IsAssembly;
            IsPublic = methodInfo.IsPublic;
            IsSpecialName = methodInfo.IsSpecialName;

            ReflectedType = new TypeWrapper(methodInfo.ReflectedType);
            DeclaringType = new TypeWrapper(methodInfo.DeclaringType);
        }

        public MethodInfo MethodInfo { get; }
        public bool IsPrivate { get; }
        public bool IsFamily { get; }
        public bool IsAssembly { get; }
        public bool IsPublic { get; }
        public bool IsSpecialName { get; }
        public ITypeWrapper ReflectedType { get; }
        public ITypeWrapper DeclaringType { get; }

        public IEnumerable<IAttributeWrapper> GetCustomAttributes()
        {
            return MethodInfo.GetCustomAttributes().Select(x => new AttributeWrapper(x)).ToArray();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            return MethodInfo.GetCustomAttributes(attributeType, inherit);
        }

        public override string ToString()
        {
            return MethodInfo.ToString();
        }
    }
}