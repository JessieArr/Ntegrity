using System;
using System.Reflection;

namespace Ntegrity.Models.Reflection
{
    public interface ITypeWrapper
    {
        Type Type { get; }
        string FullName { get; }
        string Name { get; }
        string Namespace { get; }

        bool IsClass { get; }
        bool IsInterface { get; }
        bool IsEnum { get; }
        bool IsValueType { get; }
        bool IsNestedPrivate { get; }
        bool IsVisible { get; }
        bool IsPublic { get; }
        bool IsNotPublic { get; }
        bool IsNestedPublic { get; }
        bool IsNestedFamily { get; }
        bool IsNestedAssembly { get; }
        bool IsSealed { get; }
        bool IsAbstract { get; }

        string AssemblyQualifiedName { get; }

        ITypeWrapper BaseType { get; }
        ITypeWrapper[] GetInterfaces();
        object[] GetCustomAttributes(bool inherit);
        IConstructorInfoWrapper[] GetConstructors(BindingFlags bindingAttr);
        IMethodInfoWrapper[] GetMethods();
        IPropertyInfoWrapper[] GetProperties(BindingFlags bindingAttr);
        IFieldInfoWrapper[] GetFields();
    }
}