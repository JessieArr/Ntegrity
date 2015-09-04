using System.IO;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	public class GitSourceControlHelperSystemTests
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
            var x = Directory.GetCurrentDirectory();
            var branch = _SUT.GetCurrentBranch();
			Assert.That(branch.StartsWith("* master"));
		}
	}
}
