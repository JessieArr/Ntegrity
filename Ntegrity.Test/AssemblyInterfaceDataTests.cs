using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ntegrity.TestTargetAssembly;

namespace Ntegrity.Test
{
	[TestFixture]
	public class AssemblyInterfaceDataTests
	{
		[SetUp]
		public void Init()
		{
			
		}

        [Explicit]
        [Test]
        public void BuildAssemblyInterfaceData_ForTargetAssembly()
        {
            var SUT = new AssemblyInterfaceData(typeof(PublicClass));

            var readable = SUT.GenerateHumanReadableInterfaceDefinition();
            File.WriteAllText("../../SampleOutput/TestTargetAssembly.Interface.txt", readable);
        }

        [Test]
        public void BuildAssemblyInterfaceData_ExcludePrivate_ReturnsNoPrivateFields()
        {
            var SUT = new AssemblyInterfaceData(typeof(PublicClass));

            var readable = SUT.GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings()
            {
                ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Protected
            });
            Assert.That(readable.IndexOf("private") == -1);
        }

        [Test]
        public void BuildAssemblyInterfaceData_ExcludeProtected_ReturnsNoPrivateFields()
        {
            var SUT = new AssemblyInterfaceData(typeof(PublicClass));

            var readable = SUT.GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings()
            {
                ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Internal
            });
            Assert.That(readable.IndexOf("private") == -1);
            Assert.That(readable.IndexOf("protected") == -1);
        }

        [Test]
        public void BuildAssemblyInterfaceData_ExcludePublic_ReturnsNoPrivateFields()
        {
            var SUT = new AssemblyInterfaceData(typeof(PublicClass));

            var readable = SUT.GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings()
            {
                ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Public
            });
            Assert.That(readable.IndexOf("private") == -1);
            Assert.That(readable.IndexOf("protected") == -1);
            Assert.That(readable.IndexOf("internal") == -1);
        }
    }
}
