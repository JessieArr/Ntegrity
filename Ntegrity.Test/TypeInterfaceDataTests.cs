﻿using System;
using System.Linq;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	class TypeInterfaceDataTests
	{
		[SetUp]
		public void init()
		{
		}

		[Test]
		public void Constructor_IdentifiesClassTypesCorrectly()
		{
			var SUT = new TypeInterfaceData(typeof(PublicClass));
			Assert.That(SUT.Type == TypeEnum.Class);
		}

		[Test]
		public void Constructor_IdentifiesInterfaceTypesCorrectly()
		{
			var SUT = new TypeInterfaceData(typeof(IPublicInterface));
			Assert.That(SUT.Type == TypeEnum.Interface);
		}

		[Test]
		public void Constructor_IdentifiesEnumTypesCorrectly()
		{
			var SUT = new TypeInterfaceData(typeof(PublicEnum));
			Assert.That(SUT.Type == TypeEnum.Enum);
		}

		[Test]
		public void Constructor_IdentifiesStructTypesCorrectly()
		{
			var SUT = new TypeInterfaceData(typeof(PublicStruct));
			Assert.That(SUT.Type == TypeEnum.Struct);
		}

		[Test]
		public void Constructor_IdentifiesClass_PublicAccessCorrectly()
		{
			var SUT = new TypeInterfaceData(typeof(PublicClass));
			Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
		}

		[Test]
		public void Constructor_IdentifiesClass_InternalAccessCorrectly()
		{
			var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "InternalClass");
			var SUT = new TypeInterfaceData(internalClass);
			Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
		}

		[Test]
		public void Constructor_IdentifiesClass_NestedPrivateAccessCorrectly()
		{
			var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPrivateClass");
			var SUT = new TypeInterfaceData(internalClass);
			Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
		}

		[Test]
		public void Constructor_IdentifiesClass_NestedProtectedAccessCorrectly()
		{
			var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedProtectedClass");
			var SUT = new TypeInterfaceData(internalClass);
			Assert.That(SUT.AccessLevel == AccessLevelEnum.Protected);
		}

		[Test]
		public void Constructor_SetsClass_IsStatic()
		{
			var SUT = new TypeInterfaceData(typeof(StaticClass));
			Assert.That(SUT.IsStatic);
			var SUT2 = new TypeInterfaceData(typeof(PublicClass));
			Assert.That(!SUT2.IsStatic);
		}

		[Test]
		public void ToString_BuildsCorrectString_ForPublicSealedClass()
		{
			var SUT = new TypeInterfaceData(typeof(PublicSealedClass));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("public sealed class Ntegrity.TestTargetAssembly.PublicSealedClass"));
		}

		[Test]
		public void ToString_BuildsCorrectString_ForPublicClass()
		{
			var SUT = new TypeInterfaceData(typeof(PublicClass));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("public class Ntegrity.TestTargetAssembly.PublicClass"));
		}

        [Test]
        public void ToString_BuildsCorrectString_ForInternalAbstractClass()
        {
            var type = typeof(PublicClass).Assembly.DefinedTypes.Single(x => x.Name == "InternalAbstractClass");
            var SUT = new TypeInterfaceData(type);
            var stringRepresentation = SUT.ToString();
            Assert.That(stringRepresentation.StartsWith("internal abstract class Ntegrity.TestTargetAssembly.InternalAbstractClass"));
        }

        [Test]
        public void NoInheritance_InheritsFrom_IsNull()
        {
            var type = typeof(PublicBaseClass);
            var SUT = new TypeInterfaceData(type);
            Assert.That(String.IsNullOrEmpty(SUT.InheritsFrom));
        }

        [Test]
        public void NoInterfaces_ImplementsInterfaces_IsEmpty()
        {
            var type = typeof(PublicBaseClass);
            var SUT = new TypeInterfaceData(type);
            Assert.That(SUT.ImplementsInterfaces.Count == 0);
        }

        [Test]
        public void Interface_ImplementsInterface_IsNotEmpty()
        {
            var type = typeof(IPublicChildInterface);
            var SUT = new TypeInterfaceData(type);
            Assert.That(SUT.ImplementsInterfaces.Count == 1);
            Assert.That(SUT.ImplementsInterfaces.Any(
                x => String.Equals(x, "Ntegrity.TestTargetAssembly.IPublicInterface")));
        }

        [Test]
        public void ChildClass_Inherits_BaseClass()
        {
            var type = typeof(PublicChildClass);
            var SUT = new TypeInterfaceData(type);
            Assert.That(SUT.InheritsFrom == "Ntegrity.TestTargetAssembly.PublicBaseClass");
        }

        [Test]
        public void ChildClass_Implements_Interfaces()
        {
            var type = typeof(PublicChildClass);
            var SUT = new TypeInterfaceData(type);
            Assert.That(SUT.ImplementsInterfaces.Count == 2);
            Assert.That(SUT.ImplementsInterfaces.Any(
                x => String.Equals(x, "Ntegrity.TestTargetAssembly.IPublicInterface",
                StringComparison.OrdinalIgnoreCase)));
            Assert.That(SUT.ImplementsInterfaces.Any(
                x => String.Equals(x, "Ntegrity.TestTargetAssembly.IInternalInterface",
                StringComparison.OrdinalIgnoreCase)));
        }
    }
}
