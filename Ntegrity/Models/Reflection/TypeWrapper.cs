using System;
using System.Linq;
using System.Reflection;

namespace Ntegrity.Models.Reflection
{
    public class TypeWrapper : ITypeWrapper
    {
        public TypeWrapper(Type type)
        {
            Type = type;
            FullName = type.FullName;
            Name = type.Name;
            Namespace = type.Namespace;
            IsClass = type.IsClass;
            IsInterface = type.IsInterface;
            IsEnum = type.IsEnum;
            IsValueType = type.IsValueType;
            IsNestedPrivate = type.IsNestedPrivate;
            IsVisible = type.IsVisible;
            IsPublic = type.IsPublic;
            IsNotPublic = type.IsNotPublic;
            IsNestedPublic = type.IsNestedPublic;
            IsNestedFamily = type.IsNestedFamily;
            IsNestedAssembly = type.IsNestedAssembly;
            IsSealed = type.IsSealed;
            IsAbstract = type.IsAbstract;
            AssemblyQualifiedName = type.AssemblyQualifiedName;

            if (type.BaseType != null)
            {
                BaseType = new TypeWrapper(type.BaseType);
            }
            _Interfaces = type.GetInterfaces().Select(x => new TypeWrapper(x)).ToArray();
        }

        public Type Type { get; }
        public string FullName { get; }
        public string Name { get; }
        public string Namespace { get; }
        public bool IsClass { get; }
        public bool IsInterface { get; }
        public bool IsEnum { get; }
        public bool IsValueType { get; }
        public bool IsNestedPrivate { get; }
        public bool IsVisible { get; }
        public bool IsPublic { get; }
        public bool IsNotPublic { get; }
        public bool IsNestedPublic { get; }
        public bool IsNestedFamily { get; }
        public bool IsNestedAssembly { get; }
        public bool IsSealed { get; }
        public bool IsAbstract { get; }
        public string AssemblyQualifiedName { get; }
        public ITypeWrapper BaseType { get; }

        private ITypeWrapper[] _Interfaces;
        public ITypeWrapper[] GetInterfaces()
        {
            return _Interfaces;
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            return Type.GetCustomAttributes(inherit);
        }

        public IConstructorInfoWrapper[] GetConstructors(BindingFlags bindingAttr)
        {
            return Type.GetConstructors(bindingAttr).Select(x => new ConstructorInfoWrapper(x)).ToArray();
        }

        public IMethodInfoWrapper[] GetMethods()
        {
            return Type.GetMethods().Select(x => new MethodInfoWrapper(x)).ToArray();
        }

        public IPropertyInfoWrapper[] GetProperties(BindingFlags bindingAttr)
        {
            return Type.GetProperties(bindingAttr).Select(x => new PropertyInfoWrapper(x)).ToArray();
        }

        public IFieldInfoWrapper[] GetFields()
        {
            return Type.GetFields().Select(x => new FieldInfoWrapper(x)).ToArray();
        }
    }
}