using System;
using System.Linq;
using Ntegrity.Models;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	class ClassTypeDataTests
	{
		[SetUp]
		public void init()
		{
		}

        [Test]
        public void Constructor_IdentifiesClassTypesCorrectly()
        {
            var SUT = new ClassTypeData(typeof(PublicClass));
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedInterface()
        {
            var SUT = new ClassTypeData(typeof(IPublicInterface));
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedEnum()
        {
            var SUT = new ClassTypeData(typeof(PublicEnum));
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedStruct()
        {
            var SUT = new ClassTypeData(typeof(PublicStruct));
        }

        [Test]
        public void Constructor_IdentifiesClass_PublicAccessCorrectly()
        {
            var SUT = new ClassTypeData(typeof(PublicClass));
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
        public void Constructor_IdentifiesClass_InternalAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "InternalClass");
            var SUT = new ClassTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
        public void Constructor_IdentifiesClass_NestedPrivateAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPrivateClass");
            var SUT = new ClassTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
        public void Constructor_IdentifiesClass_NestedProtectedAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedProtectedClass");
            var SUT = new ClassTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Protected);
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
        public void Constructor_IdentifiesClass_NestedInternalAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedInternalClass");
            var SUT = new ClassTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
        public void Constructor_IdentifiesClass_NestedPublicAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPublicClass");
            var SUT = new ClassTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Class);
        }

        [Test]
		public void Constructor_SetsClass_IsStatic()
		{
			var SUT = new ClassTypeData(typeof(StaticClass));
			Assert.That(SUT.IsStatic);
			var SUT2 = new ClassTypeData(typeof(PublicClass));
			Assert.That(!SUT2.IsStatic);
		}

		[Test]
		public void ToString_BuildsCorrectString_ForPublicSealedClass()
		{
			var SUT = new ClassTypeData(typeof(PublicSealedClass));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("\tpublic sealed class Ntegrity.TestTargetAssembly.PublicSealedClass"));
		}

		[Test]
		public void ToString_BuildsCorrectString_ForPublicClass()
		{
			var SUT = new ClassTypeData(typeof(PublicClass));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("\tpublic class Ntegrity.TestTargetAssembly.PublicClass"));
		}

        [Test]
        public void ToString_BuildsCorrectString_ForInternalAbstractClass()
        {
            var type = typeof(PublicClass).Assembly.DefinedTypes.Single(x => x.Name == "InternalAbstractClass");
            var SUT = new ClassTypeData(type);
            var stringRepresentation = SUT.ToString();
            Assert.That(stringRepresentation.StartsWith("\tinternal abstract class Ntegrity.TestTargetAssembly.InternalAbstractClass"));
        }

        [Test]
        public void NoInheritance_InheritsFrom_IsNull()
        {
            var type = typeof(PublicBaseClass);
            var SUT = new ClassTypeData(type);
            Assert.That(String.IsNullOrEmpty(SUT.InheritsFrom));
        }

        [Test]
        public void NoInterfaces_ImplementsInterfaces_IsEmpty()
        {
            var type = typeof(PublicBaseClass);
            var SUT = new ClassTypeData(type);
            Assert.That(SUT.ImplementsInterfaces.Count == 0);
        }

        [Test]
        public void ChildClass_Inherits_BaseClass()
        {
            var type = typeof(PublicChildClass);
            var SUT = new ClassTypeData(type);
            Assert.That(SUT.InheritsFrom == "Ntegrity.TestTargetAssembly.PublicBaseClass");
        }

        [Test]
        public void ChildClass_Implements_Interfaces()
        {
            var type = typeof(PublicChildClass);
            var SUT = new ClassTypeData(type);
            Assert.That(SUT.ImplementsInterfaces.Count == 2);
            Assert.That(SUT.ImplementsInterfaces.Any(
                x => String.Equals(x, "Ntegrity.TestTargetAssembly.IPublicInterface",
                StringComparison.OrdinalIgnoreCase)));
            Assert.That(SUT.ImplementsInterfaces.Any(
                x => String.Equals(x, "Ntegrity.TestTargetAssembly.IInternalInterface",
                StringComparison.OrdinalIgnoreCase)));
        }

	    [Test]
	    public void Constructor_Parses_ClassStringInput()
	    {
	        var testType = new ClassTypeData(typeof(PublicClassWithAttributes));
	        var testString = testType.ToString();

            var SUT = new ClassTypeData(testString);

            Assert.NotNull(SUT);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.AttributeData.Count == testType.AttributeData.Count);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
            Assert.That(SUT.Type == testType.Type);
        }
    }
}
