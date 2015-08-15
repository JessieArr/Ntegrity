using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntegrity.TestTargetAssembly
{
	public class TestAttributeAttribute : Attribute
	{
		public string Text;
		public TestAttributeAttribute(string text)
		{
			Text = text;
		}
	}
}
