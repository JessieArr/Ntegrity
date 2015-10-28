using System;
using System.Linq;
using Ntegrity.Models;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	class EnumTypeDataTests
	{
		[SetUp]
		public void init()
		{
		}

		[Test]
		public void Constructor_IdentifiesClassTypesCorrectly()
		{
			var SUT = new EnumTypeData(typeof(PublicEnum));
			Assert.That(SUT.Type == TypeEnum.Enum);
		}

        [Test]
        public void Constructor_IdentifiesClass_PublicAccessCorrectly()
        {
            var SUT = new EnumTypeData(typeof(PublicEnum));
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Enum);
        }

        [Test]
        public void Constructor_IdentifiesClass_InternalAccessCorrectly()
        {
            var internalEnum = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "InternalEnum");
            var SUT = new EnumTypeData(internalEnum);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void Constructor_IdentifiesClass_NestedPrivateAccessCorrectly()
        {
            var internalClass = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPrivateEnum");
            var SUT = new EnumTypeData(internalClass);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void Constructor_IdentifiesStruct_NestedProtectedAccessCorrectly()
        {
            var internalStruct = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedProtectedEnum");
            var SUT = new EnumTypeData(internalStruct);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void Constructor_IdentifiesStruct_NestedInternalAccessCorrectly()
        {
            var internalStruct = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedInternalEnum");
            var SUT = new EnumTypeData(internalStruct);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void Constructor_IdentifiesStruct_NestedPublicAccessCorrectly()
        {
            var internalEnum = typeof(PublicStruct).Assembly.DefinedTypes.Single(x => x.Name == "NestedPublicEnum");
            var SUT = new EnumTypeData(internalEnum);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.Type == TypeEnum.Enum);
        }

		[Test]
		public void ToString_BuildsCorrectString_ForPublicEnum()
		{
			var SUT = new EnumTypeData(typeof(PublicEnum));
			var stringRepresentation = SUT.ToString();
			Assert.That(stringRepresentation.StartsWith("\tpublic enum Ntegrity.TestTargetAssembly.PublicEnum"));
		}

	    [Test]
	    public void Constructor_Parses_ClassStringInput()
	    {
	        var testType = new EnumTypeData(typeof(PublicEnumWithAttributes));
	        var testString = testType.ToString();

            var SUT = new EnumTypeData(testString);

            Assert.NotNull(SUT);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
            Assert.That(SUT.AttributeData.Count == testType.AttributeData.Count);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
            Assert.That(SUT.Type == testType.Type);
        }
    }
}
