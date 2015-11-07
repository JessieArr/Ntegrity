using System;
using System.Collections.Generic;
using System.Linq;
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

        private string _RemovedMethodName;
        private Mock<IMethodData> _RemovedMethod;
        private string _AddedMethodName;
        private Mock<IMethodData> _AddedMethod;

        [SetUp]
        public void BeforeEachTest()
        {
            _RemovedAttributeName = "Namespace.RemovedAttribute";
            _RemovedAttribute = new Mock<IAttributeData>();
            _RemovedAttribute.SetupGet(x => x.Name).Returns(_RemovedAttributeName);

            _AddedAttributeName = "Namespace.AddedAttribute";
            _AddedAttribute = new Mock<IAttributeData>();
            _AddedAttribute.SetupGet(x => x.Name).Returns(_AddedAttributeName);

            _RemovedFieldName = "Namespace.RemovedField";
            _RemovedField = new Mock<IFieldData>();
            _RemovedField.SetupGet(x => x.FieldSignature).Returns(_RemovedFieldName);

            _AddedFieldName = "Namespace.AddedField";
            _AddedField = new Mock<IFieldData>();
            _AddedField.SetupGet(x => x.FieldSignature).Returns(_AddedFieldName);

            _RemovedMethodName = "Namespace.RemovedMethod";
            _RemovedMethod = new Mock<IMethodData>();
            _RemovedMethod.SetupGet(x => x.MethodSignature).Returns(_RemovedMethodName);

            _AddedMethodName = "Namespace.AddedMethod";
            _AddedMethod = new Mock<IMethodData>();
            _AddedMethod.SetupGet(x => x.MethodSignature).Returns(_AddedMethodName);
        }

        [Test]
        public void Test()
        {
            var enumv1Mock = new Mock<IEnumTypeData>();
            enumv1Mock.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    _RemovedAttribute.Object
                });
            enumv1Mock.SetupGet(x => x.FieldData)
                .Returns(new List<IFieldData>()
                {
                    _RemovedField.Object
                });
            enumv1Mock.SetupGet(x => x.MethodData)
                .Returns(new List<IMethodData>()
                {
	                _RemovedMethod.Object
                });

            var enumv2Mock = new Mock<IEnumTypeData>();
            enumv2Mock.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    _AddedAttribute.Object
                });
            enumv2Mock.SetupGet(x => x.FieldData)
                .Returns(new List<IFieldData>()
                {
	                _AddedField.Object
                });
            enumv2Mock.SetupGet(x => x.MethodData)
                .Returns(new List<IMethodData>()
                {
	                _AddedMethod.Object
                });

            var SUT = new EnumTypeDiff(enumv1Mock.Object, enumv2Mock.Object);
            Assert.That(SUT.AddedAttributes.Count == 1);
            Assert.That(SUT.RemovedAttributes.Count == 1);
            Assert.That(SUT.AddedFields.Count == 1);
            Assert.That(SUT.RemovedFields.Count == 1);
            Assert.That(SUT.AddedMethods.Count == 1);
            Assert.That(SUT.RemovedMethods.Count == 1);
        }
    }
}
