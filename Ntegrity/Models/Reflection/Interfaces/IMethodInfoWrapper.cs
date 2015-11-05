using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity.Models.Reflection.Interfaces
{
    public interface IMethodInfoWrapper
    {
        MethodInfo MethodInfo { get; }

        bool IsPrivate { get; }
        bool IsFamily { get; }
        bool IsAssembly { get; }
        bool IsPublic { get; }
        bool IsSpecialName { get; }

        ITypeWrapper ReflectedType { get; }
        ITypeWrapper DeclaringType { get; }

        IEnumerable<IAttributeWrapper> GetCustomAttributes();
        object[] GetCustomAttributes(Type attributeType, bool inherit);

        string ToString();
    }
}