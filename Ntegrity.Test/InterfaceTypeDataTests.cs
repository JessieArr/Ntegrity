using System;
using System.Linq;
using Ntegrity.Models;
using Ntegrity.Models.Reflection;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	class InterfaceTypeDataTests
    {
		[SetUp]
		public void init()
		{
		}

		[Test]
		public void Constructor_IdentifiesClassTypesCorrectly()
		{
			var SUT = new InterfaceTypeData(new TypeWrapper(typeof(IPublicInterface)));
			Assert.That(SUT.Type == TypeEnum.Interface);
		}

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedClass()
        {
            var SUT = new InterfaceTypeData(new TypeWrapper(typeof(PublicClass)));
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedStruct()
        {
            var SUT = new InterfaceTypeData(new TypeWrapper(typeof(PublicStruct)));
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedEnum()
        {
            var SUT = new InterfaceTypeData(new TypeWrapper(typeof(PublicEnum)));
        }

        [Test]
        public void Constructor_Identifies_PublicAccessCorrectly()
        {
            var SUT = new InterfaceTypeData(new TypeWrapper(typeof(IPublicInterface)));
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Interface);
        }

        [Test]
        public void Constructor_Identifies_InternalAccessCorrectly()
        {
            var internalInterface = new TypeWrapper(typeof(IPublicInterface).Assembly.DefinedTypes.Single(x => x.Name == "IInternalInterface"));
            var SUT = new InterfaceTypeData(internalInterface);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void Constructor_Identifies_NestedPrivateAccessCorrectly()
        {
            var internalClass = new TypeWrapper(typeof(IPublicInterface).Assembly.DefinedTypes.Single(x => x.Name == "INestedPrivateInterface"));
            var SUT = new InterfaceTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void Constructor_Identifies_NestedProtectedAccessCorrectly()
        {
            var internalInterface = new TypeWrapper(typeof(IPublicInterface).Assembly.DefinedTypes.Single(x => x.Name == "INestedProtectedInterface"));
            var SUT = new InterfaceTypeData(internalInterface);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void Constructor_Identifies_NestedInternalAccessCorrectly()
        {
            var internalInterface = new TypeWrapper(typeof(IPublicInterface).Assembly.DefinedTypes.Single(x => x.Name == "INestedInternalInterface"));
            var SUT = new InterfaceTypeData(internalInterface);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void Constructor_Identifies_NestedPublicAccessCorrectly()
        {
            var internalInterface = new TypeWrapper(typeof(IPublicInterface).Assembly.DefinedTypes.Single(x => x.Name == "INestedPublicInterface"));
            var SUT = new InterfaceTypeData(internalInterface);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Interface);
        }

		[Test]
		public void ToString_BuildsCorrectString_ForPublicInterface()
		{
			var SUT = new InterfaceTypeData(new TypeWrapper(typeof(IPublicInterface)));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("\tpublic interface Ntegrity.TestTargetAssembly.IPublicInterface"));
		}

	    [Test]
	    public void Constructor_Parses_InterfaceStringInput()
	    {
	        var testType = new InterfaceTypeData(new TypeWrapper(typeof(IPublicInterfaceWithAttributes)));
	        var testString = testType.ToString();

            var SUT = new InterfaceTypeData(testString);

            Assert.NotNull(SUT);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.AttributeData.Count == testType.AttributeData.Count);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
            Assert.That(SUT.Type == testType.Type);
        }
    }
}
