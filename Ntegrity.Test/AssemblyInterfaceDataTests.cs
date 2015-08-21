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
	}
}
