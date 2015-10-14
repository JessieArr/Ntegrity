using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
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

        [Test]
        public void StringConstructor_ProperlyParsesOutput()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof (PublicClass));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition();

            var SUT = new AssemblyInterfaceData(testString);

            Assert.That(SUT.Name == testInterfaceData.Name);
            Assert.That(SUT.Version == testInterfaceData.Version);
            Assert.That(SUT.CLRVersion == testInterfaceData.CLRVersion);
            Assert.That(SUT.ReferencedAssemblies.Count == testInterfaceData.ReferencedAssemblies.Count);
            Assert.That(SUT.ReferencedAssemblies.All(x => testInterfaceData.ReferencedAssemblies.Any(y => y == x)));
        }
    }
}
