using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ntegrity.Models;
using Ntegrity.Models.Diff;
using Ntegrity.Models.Interfaces;
using NUnit.Framework;

namespace Ntegrity.Test
{
    [TestFixture]
    public class EnumDiffTests
    {
        private string _RemovedAttributeName;
        private Mock<IAttributeData> _RemovedAttribute;
        private string _AddedAttributeName;
        private Mock<IAttributeData> _AddedAttribute;

        private string _RemovedFieldName;
        private Mock<IFieldData> _RemovedField;
		private string _AddedFieldName;
		private Mock<IFieldData> _AddedField;
		private string _ModifiedFieldName;
		private Mock<IFieldData> _ModifiedFieldBefore;
		private Mock<IFieldData> _ModifiedFieldAfter;

		private string _RemovedMethodName;
		private Mock<IMethodData> _RemovedMethod;
		private string _AddedMethodName;
		private Mock<IMethodData> _AddedMethod;
		private string _ModifiedMethodName;
		private Mock<IMethodData> _ModifiedMethodBefore;
		private Mock<IMethodData> _ModifiedMethodAfter;


		private Mock<IEnumTypeData> _EnumV1;
		private Mock<IEnumTypeData> _EnumV2;

		[SetUp]
        public void BeforeEachTest()
        {
            _RemovedAttributeName = "Namespace.RemovedAttribute";
            _RemovedAttribute = TestMockGenerator.GetMockAttribute(_RemovedAttributeName);

            _AddedAttributeName = "Namespace.AddedAttribute";
            _AddedAttribute = TestMockGenerator.GetMockAttribute(_AddedAttributeName);

            _RemovedFieldName = "Namespace.RemovedField";
            _RemovedField = TestMockGenerator.GetMockField(_RemovedFieldName);

			_AddedFieldName = "Namespace.AddedField";
			_AddedField = TestMockGenerator.GetMockField(_AddedFieldName);

			_ModifiedFieldName = "Namespace.ModifiedField";
			_ModifiedFieldBefore = TestMockGenerator.GetMockField(_ModifiedFieldName,
				new List<IAttributeData>()
				{
					_RemovedAttribute.Object
				});

			_ModifiedFieldAfter = TestMockGenerator.GetMockField(_ModifiedFieldName,
				new List<IAttributeData>()
				{
					_AddedAttribute.Object
				});

			_RemovedMethodName = "Namespace.RemovedMethod";
            _RemovedMethod = TestMockGenerator.GetMockMethod(_RemovedMethodName);

            _AddedMethodName = "Namespace.AddedMethod";
            _AddedMethod = TestMockGenerator.GetMockMethod(_AddedMethodName);

			_ModifiedMethodName = "Namespace.ModifiedMethod";
			_ModifiedMethodBefore = TestMockGenerator.GetMockMethod(_ModifiedMethodName,
				new List<IAttributeData>()
				{
					_RemovedAttribute.Object
				});

			_ModifiedMethodAfter = TestMockGenerator.GetMockMethod(_ModifiedMethodName,
				new List<IAttributeData>()
				{
					_AddedAttribute.Object
				});

			_EnumV1 = new Mock<IEnumTypeData>();
			_EnumV1.SetupGet(x => x.Name).Returns("Namespace.TestEnum");
            _EnumV1.SetupGet(x => x.AttributeData)
				.Returns(new List<IAttributeData>()
				{
					_RemovedAttribute.Object
				});
			_EnumV1.SetupGet(x => x.FieldData)
				.Returns(new List<IFieldData>()
				{
					_RemovedField.Object,
					_ModifiedFieldBefore.Object
				});
			_EnumV1.SetupGet(x => x.MethodData)
				.Returns(new List<IMethodData>()
				{
					_RemovedMethod.Object,
					_ModifiedMethodBefore.Object
				});

			_EnumV2 = new Mock<IEnumTypeData>();
			_EnumV2.SetupGet(x => x.Name).Returns("Namespace.TestEnum");
			_EnumV2.SetupGet(x => x.AttributeData)
				.Returns(new List<IAttributeData>()
				{
					_AddedAttribute.Object
				});
			_EnumV2.SetupGet(x => x.FieldData)
				.Returns(new List<IFieldData>()
				{
					_AddedField.Object,
					_ModifiedFieldAfter.Object
				});
			_EnumV2.SetupGet(x => x.MethodData)
				.Returns(new List<IMethodData>()
				{
					_AddedMethod.Object,
					_ModifiedMethodAfter.Object
				});
		}

		[Test]
		public void EnumTypeDiff_CorrectlyCalculates_Attributes()
		{
			var SUT = new EnumTypeDiff(_EnumV1.Object, _EnumV2.Object);

			Assert.That(SUT.AddedAttributes.Count == 1);
			Assert.That(SUT.RemovedAttributes.Count == 1);
			Assert.That(SUT.AddedAttributes.Any(x => x.Name == _AddedAttributeName));
			Assert.That(SUT.RemovedAttributes.Any(x => x.Name == _RemovedAttributeName));
		}

		[Test]
		public void EnumTypeDiff_CorrectlyCalculates_Fields()
		{
			var SUT = new EnumTypeDiff(_EnumV1.Object, _EnumV2.Object);

			Assert.That(SUT.AddedFields.Count == 1);
			Assert.That(SUT.RemovedFields.Count == 1);
			Assert.That(SUT.ModifiedFields.Count == 1);
			Assert.That(SUT.AddedFields.Any(x => x.FieldSignature == _AddedFieldName));
			Assert.That(SUT.RemovedFields.Any(x => x.FieldSignature == _RemovedFieldName));
			Assert.That(SUT.ModifiedFields.Any(x => x.FieldSignature == _ModifiedFieldName));
		}

		[Test]
		public void EnumTypeDiff_CorrectlyCalculates_Methods()
		{
			var SUT = new EnumTypeDiff(_EnumV1.Object, _EnumV2.Object);

			Assert.That(SUT.AddedMethods.Count == 1);
			Assert.That(SUT.RemovedMethods.Count == 1);
			Assert.That(SUT.AddedMethods.Any(x => x.MethodSignature == _AddedMethodName));
			Assert.That(SUT.RemovedMethods.Any(x => x.MethodSignature == _RemovedMethodName));
			Assert.That(SUT.ModifiedMethods.Any(x => x.MethodSignature == _ModifiedMethodName));
		}

		[Test]
		public void EnumTypeDiff_ChangedCollections_WithNoChanges()
		{
			var enumWithNoChanges = new Mock<IEnumTypeData>();
			enumWithNoChanges.SetupGet(x => x.Name).Returns("Namespace.TestEnum");
			enumWithNoChanges.SetupGet(x => x.AttributeData)
				.Returns(new List<IAttributeData>()
				{
					_RemovedAttribute.Object
				});
			enumWithNoChanges.SetupGet(x => x.FieldData)
				.Returns(new List<IFieldData>()
				{
					_RemovedField.Object,
					_ModifiedFieldBefore.Object
				});
			enumWithNoChanges.SetupGet(x => x.MethodData)
				.Returns(new List<IMethodData>()
				{
					_RemovedMethod.Object,
					_ModifiedMethodBefore.Object
				});
			var SUT = new EnumTypeDiff(enumWithNoChanges.Object, enumWithNoChanges.Object);

			Assert.That(SUT.ModifiedFields.Count == 0);
			Assert.That(SUT.ModifiedMethods.Count == 0);
		}

		[Test]
		public void ToString_DoesNotThrow()
		{
			var SUT = new EnumTypeDiff(_EnumV1.Object, _EnumV2.Object);
			var output = SUT.ToString();
		}

		[Explicit("This test writes to disk for purposes of testing string output.")]
		[Test]
		public void GenerateEnumDiffTextOutput()
		{
			var SUT = new EnumTypeDiff(_EnumV1.Object, _EnumV2.Object);
			var output = SUT.ToString();

			File.WriteAllText("../../SampleOutput/TestArea/EnumDiffTestOutput.txt", output);
		}
	}
}
