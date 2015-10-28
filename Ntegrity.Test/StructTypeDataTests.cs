using System;
using System.Linq;
using Ntegrity.Models;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	class StructTypeDataTests
    {
		[SetUp]
		public void init()
		{
		}

		[Test]
		public void Constructor_IdentifiesClassTypesCorrectly()
		{
			var SUT = new StructTypeData(typeof(PublicStruct));
			Assert.That(SUT.Type == TypeEnum.Struct);
		}

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedClass()
        {
            var SUT = new StructTypeData(typeof(PublicClass));
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedInterface()
        {
            var SUT = new StructTypeData(typeof(IPublicInterface));
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void Constructor_Throws_WhenPassedEnum()
        {
            var SUT = new StructTypeData(typeof(PublicEnum));
        }

        [Test]
        public void Constructor_Identifies_PublicAccessCorrectly()
        {
            var SUT = new StructTypeData(typeof(PublicStruct));
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Struct);
        }

        [Test]
        public void Constructor_Identifies_InternalAccessCorrectly()
        {
            var internalStruct = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "InternalStruct");
            var SUT = new StructTypeData(internalStruct);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void Constructor_Identifies_NestedPrivateAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPrivateStruct");
            var SUT = new StructTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void Constructor_Identifies_NestedProtectedAccessCorrectly()
        {
            var internalStruct = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedProtectedStruct");
            var SUT = new StructTypeData(internalStruct);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void Constructor_Identifies_NestedInternalAccessCorrectly()
        {
            var internalStruct = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedInternalStruct");
            var SUT = new StructTypeData(internalStruct);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void Constructor_Identifies_NestedPublicAccessCorrectly()
        {
            var internalStruct = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPublicStruct");
            var SUT = new StructTypeData(internalStruct);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Struct);
        }

		[Test]
		public void ToString_BuildsCorrectString_ForPublicStruct()
		{
			var SUT = new StructTypeData(typeof(PublicStruct));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("\tpublic struct Ntegrity.TestTargetAssembly.PublicStruct"));
		}

	    [Test]
	    public void Constructor_Parses_StructStringInput()
	    {
	        var testType = new StructTypeData(typeof(PublicStructWithAttributes));
	        var testString = testType.ToString();

            var SUT = new StructTypeData(testString);

            Assert.NotNull(SUT);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.AttributeData.Count == testType.AttributeData.Count);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
            Assert.That(SUT.Type == testType.Type);
        }
    }
}
