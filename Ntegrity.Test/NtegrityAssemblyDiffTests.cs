using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.Models.Reflection;
using NUnit.Framework;

namespace Ntegrity.Test
{
    [TestFixture]
    public class NtegrityAssemblyDiffTests
    {
        [Explicit]
        [Test]
        public void BuildAssemblyDiff_Forv1Andv2()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var v1Assembly = new AssemblyWrapper(Assembly.LoadFile(currentDirectory
                + "\\TestAssemblyVersions\\VersionTestAssembly.1.0.0.0.dll"));
            var v1AssemblyInterfaceData = new AssemblyInterfaceData(v1Assembly);

            var v2Assembly = new AssemblyWrapper(Assembly.LoadFile(currentDirectory
                + "\\TestAssemblyVersions\\VersionTestAssembly.2.0.0.0.dll"));
            var v2AssemblyInterfaceData = new AssemblyInterfaceData(v2Assembly);

            var SUT = new NtegrityAssemblyDiff(v1AssemblyInterfaceData, v2AssemblyInterfaceData);

            var readable = SUT.ToString();
            File.WriteAllText("../../SampleOutput/AssemblyVersionDiff.txt", readable);
        }

        [Test]
        public void RealDLLTest()
        {
            var currentDirectory = Directory.GetCurrentDirectory();

            var v1Assembly = new AssemblyWrapper(Assembly.LoadFile(currentDirectory 
                + "\\TestAssemblyVersions\\VersionTestAssembly.1.0.0.0.dll"));
            var v1AssemblyInterfaceData = new AssemblyInterfaceData(v1Assembly);

            var v2Assembly = new AssemblyWrapper(Assembly.LoadFile(currentDirectory
                + "\\TestAssemblyVersions\\VersionTestAssembly.2.0.0.0.dll"));
            var v2AssemblyInterfaceData = new AssemblyInterfaceData(v2Assembly);

            var SUT = new NtegrityAssemblyDiff(v1AssemblyInterfaceData, v2AssemblyInterfaceData);

            Assert.That(SUT != null);
            Assert.That(SUT.RemovedClasses.Count == 1);
            Assert.That(SUT.AddedClasses.Count == 1);
            Assert.That(SUT.RemovedInterfaces.Count == 1);
            Assert.That(SUT.AddedInterfaces.Count == 1);
            Assert.That(SUT.RemovedStructs.Count == 1);
            Assert.That(SUT.AddedStructs.Count == 1);
            Assert.That(SUT.RemovedEnums.Count == 1);
            Assert.That(SUT.AddedEnums.Count == 1);
        }

		[Test]
		public void ToString_DoesNotThrow()
		{
			var currentDirectory = Directory.GetCurrentDirectory();

			var v1Assembly = new AssemblyWrapper(Assembly.LoadFile(currentDirectory
				+ "\\TestAssemblyVersions\\VersionTestAssembly.1.0.0.0.dll"));
			var v1AssemblyInterfaceData = new AssemblyInterfaceData(v1Assembly);

			var v2Assembly = new AssemblyWrapper(Assembly.LoadFile(currentDirectory
				+ "\\TestAssemblyVersions\\VersionTestAssembly.2.0.0.0.dll"));
			var v2AssemblyInterfaceData = new AssemblyInterfaceData(v2Assembly);

			var SUT = new NtegrityAssemblyDiff(v1AssemblyInterfaceData, v2AssemblyInterfaceData);

			Assert.That(SUT.ToString() != null);
		}
	}
}
