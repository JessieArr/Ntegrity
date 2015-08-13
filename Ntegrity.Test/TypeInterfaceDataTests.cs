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
			var SUT = new TypeInterfaceData(typeof(TargetClass));
			Assert.That(SUT.Type == TypeEnum.Class);
		}

		[Test]
		public void Constructor_IdentifiesInterfaceTypesCorrectly()
		{
			var SUT = new TypeInterfaceData(typeof(ITargetInterface));
			Assert.That(SUT.Type == TypeEnum.Interface);
		}
	}
}
