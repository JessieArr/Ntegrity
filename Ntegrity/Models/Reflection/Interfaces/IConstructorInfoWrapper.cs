using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ntegrity.Models.Reflection.Interfaces
{
    public interface IConstructorInfoWrapper
    {
        ConstructorInfo ConstructorInfo{ get; }

        bool IsPrivate { get; }
        bool IsFamily { get; }
        bool IsAssembly { get; }
        bool IsPublic { get; }

        IEnumerable<IAttributeWrapper> GetCustomAttributes();
        object[] GetCustomAttributes(Type attributeType, bool inherit);
    }
}