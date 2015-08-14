using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TestTargetAssembly;

namespace Ntegrity.Test
{
	[TestFixture]
	public class AssemblyInterfaceDataTests
	{
		[SetUp]
		public void Init()
		{
			
		}

		[Test]
		public void BuildAssemblyInterfaceData_ForTargetAssembly()
		{
			var SUT = new AssemblyInterfaceData(typeof(PublicClass));

			var readable = SUT.GenerateHumanReadableInterfaceDefinition();
		}
	}
}
