using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ntegrity.Models;
using Ntegrity.Models.Interfaces;

namespace Ntegrity.Test
{
	public static class TestMockGenerator
	{
		public static Mock<IAttributeData> GetMockAttribute(string name)
		{
			var mock = new Mock<IAttributeData>();
			mock.SetupGet(x => x.Name).Returns(name);
			return mock;
		}

		public static Mock<IAttributeData> GetMockAttribute(string name, bool isCompilerGenerated)
		{
			var mock = new Mock<IAttributeData>();
			mock.SetupGet(x => x.Name).Returns(name);
			mock.SetupGet(x => x.IsCompilerGenerated).Returns(isCompilerGenerated);
			return mock;
		}

		public static Mock<IFieldData> GetMockField(string signature)
		{
			var mock = new Mock<IFieldData>();
			mock.SetupGet(x => x.FieldSignature).Returns(signature);
			mock.SetupGet(x => x.AttributeData).Returns(new List<IAttributeData>());
			return mock;
		}

		public static Mock<IFieldData> GetMockField(string signature,
			List<IAttributeData> attributes)
		{
			var mock = new Mock<IFieldData>();
			mock.SetupGet(x => x.FieldSignature).Returns(signature);
			mock.SetupGet(x => x.AttributeData).Returns(attributes);
			return mock;
		}

		public static Mock<IFieldData> GetMockField(string signature,
			List<IAttributeData> attributes,
			AccessLevelEnum accessLevel)
		{
			var mock = new Mock<IFieldData>();
			mock.SetupGet(x => x.FieldSignature).Returns(signature);
			mock.SetupGet(x => x.AttributeData).Returns(attributes);
			mock.SetupGet(x => x.AccessLevel).Returns(accessLevel);
			return mock;
		}

		public static Mock<IMethodData> GetMockMethod(string signature)
		{
			var mock = new Mock<IMethodData>();
			mock.SetupGet(x => x.MethodSignature).Returns(signature);
			mock.SetupGet(x => x.AttributeData).Returns(new List<IAttributeData>());
			return mock;
		}

		public static Mock<IMethodData> GetMockMethod(string signature,
			List<IAttributeData> attributes)
		{
			var mock = new Mock<IMethodData>();
			mock.SetupGet(x => x.MethodSignature).Returns(signature);
			mock.SetupGet(x => x.AttributeData).Returns(attributes);
			return mock;
		}

		public static Mock<IMethodData> GetMockMethod(string signature,
			List<IAttributeData> attributes,
			AccessLevelEnum accessLevel)
		{
			var mock = new Mock<IMethodData>();
			mock.SetupGet(x => x.MethodSignature).Returns(signature);
			mock.SetupGet(x => x.AttributeData).Returns(attributes);
			mock.SetupGet(x => x.AccessLevel).Returns(accessLevel);
			return mock;
		}
	}
}
