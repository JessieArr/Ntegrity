using System;
using Ntegrity.Models;
using Ntegrity.SourceControl;
using NUnit.Framework;

namespace Ntegrity.Test
{
	[TestFixture]
	public class NtegrityTests
	{
		private Ntegrity _SUT;

		[SetUp]
		public void init()
		{
			_SUT = new Ntegrity();
		}

		[Test]
		[ExpectedException(typeof(NtegrityException))]
		public void SourceControlHelper_DefaultsToGit()
		{
			var test = _SUT.SourceControlHelper;
		}

		[Test]
		public void Constructor_CalledWithGit_InitializesGitHelper()
		{
			var SUT = new Ntegrity(SourceControlTypeEnum.Git);
			var isGit = SUT.SourceControlHelper is GitSourceControlHelper;
			Assert.That(isGit);
		}

		[Test]
		public void Constructor_CalledWithHG_InitializesHGHelper()
		{
			var SUT = new Ntegrity(SourceControlTypeEnum.Mercurial);
			var isHG = SUT.SourceControlHelper is MercurialSourceControlHelper;
			Assert.That(isHG);
		}
	}
}
