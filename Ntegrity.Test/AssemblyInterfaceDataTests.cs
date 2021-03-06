﻿using System;
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

        [Explicit]
        [Test]
        public void BuildAssemblyInterfaceData_ForNUnit()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof(TestFixtureAttribute));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings()
            {
                ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Public
            });
            File.WriteAllText("../../SampleOutput/NUnit.Interface.txt", testString);
        }

        [Explicit]
        [Test]
        public void BuildAssemblyInterfaceData_ForEntityFramework()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof(System.Data.Entity.Database));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings()
            {
                ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Public
            });
            File.WriteAllText("../../SampleOutput/EntityFramework.Interface.txt", testString);
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
            var testInterfaceData = new AssemblyInterfaceData(typeof(PublicClass));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition();

            var SUT = new AssemblyInterfaceData(testString);

            Assert.That(SUT.Name == testInterfaceData.Name);
            Assert.That(SUT.Version == testInterfaceData.Version);
            Assert.That(SUT.CLRVersion == testInterfaceData.CLRVersion);
            Assert.That(SUT.ReferencedAssemblies.Count == testInterfaceData.ReferencedAssemblies.Count);
            Assert.That(SUT.ReferencedAssemblies.All(x => testInterfaceData.ReferencedAssemblies.Any(y => y == x)));
        }

        [Test]
        public void StringOutput_DoesNotHaveTwoSpaces()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof(PublicClass));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition();

            Assert.That(!testString.Contains("  "));
        }

        [Test]
        public void StringOutput_DoesNotHaveThreeNewlines()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof(PublicClass));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition();

            Assert.That(!testString.Contains(Environment.NewLine + Environment.NewLine + Environment.NewLine));
        }

        [Test]
        public void StringOutput_ForNUnit_DoesNotThrow()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof(TestFixtureAttribute));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition();
        }

        [Test]
        public void StringOutput_ForEntityFramework_DoesNotThrow()
        {
            var testInterfaceData = new AssemblyInterfaceData(typeof(System.Data.Entity.Database));
            var testString = testInterfaceData.GenerateHumanReadableInterfaceDefinition();
        }

        [Ignore("This is the big one. Will take quite a bit of work to implement.")]
        [Test]
        public void StringOutput_GeneratedTwice_IsIdentical()
        {
            var testAssembly = new AssemblyInterfaceData(typeof(PublicClass));
            var firstString = testAssembly.GenerateHumanReadableInterfaceDefinition();

            var secondPass = new AssemblyInterfaceData(firstString);
            var secondString = secondPass.GenerateHumanReadableInterfaceDefinition();

            Assert.That(firstString.Length == secondString.Length);
            Assert.That(firstString.Equals(secondString));
        }
    }
}
