using NUnit.Framework;
using TestTargetAssembly;

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
	}
}
