using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	public class GitSourceControlHelperTests
	{
		private GitSourceControlHelper _SUT;
		[SetUp]
		public void Init()
		{
			_SUT = new GitSourceControlHelper();
		}

		[Test]
		public void GetBranch_ReturnsMaster()
		{
			var branch = _SUT.GetCurrentBranch();
			Assert.That(branch.StartsWith("* master"));
		}
	}
}
