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
    public class MethodDiffTests
    {
        [Test]
        public void HasChanged_IsSet_ForChangedField()
        {
            var oldAttributeName = "Test.OldAttribute";
            var oldAttribute = new Mock<IAttributeData>();
            oldAttribute.SetupGet(x => x.Name).Returns(oldAttributeName);

            var newAttributeName = "Test.NewAttribute";
            var newAttribute = new Mock<IAttributeData>();
            newAttribute.SetupGet(x => x.Name).Returns(newAttributeName);

            var oldField = new Mock<IMethodData>();
            oldField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    oldAttribute.Object
                });

            var newField = new Mock<IMethodData>();
            newField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    newAttribute.Object
                });

            var SUT = new MethodDiff(oldField.Object, newField.Object);
            Assert.True(SUT.HasChanged);
        }

        [Test]
        [ExpectedException(typeof(NtegrityException))]
        public void ExceptionRaise_ForDiffOfDisparateFields()
        {
            var oldFieldName = "Test.OldMethod";
            var oldField = new Mock<IMethodData>();
            oldField.SetupGet(x => x.MethodSignature)
                .Returns(oldFieldName);

            var newFieldName = "Test.NewField";
            var newField = new Mock<IMethodData>();
            newField.SetupGet(x => x.MethodSignature)
                .Returns(newFieldName);

            var SUT = new MethodDiff(oldField.Object, newField.Object);
        }

        [Test]
        public void AddedAttributes_Populated_ForNewAttribute()
        {
            var newAttributeName = "Test.NewAttribute";
            var newAttribute = new Mock<IAttributeData>();
            newAttribute.SetupGet(x => x.Name).Returns(newAttributeName);

            var oldField = new Mock<IMethodData>();
            oldField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>());

            var newField = new Mock<IMethodData>();
            newField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    newAttribute.Object
                });

            var SUT = new MethodDiff(oldField.Object, newField.Object);
            Assert.True(SUT.AddedAttributes.Any(x => x.Name == newAttributeName));
            Assert.False(SUT.RemovedAttributes.Any());
        }

        [Test]
        public void RemovedAttributes_Populated_ForRemovedAttribute()
        {
            var oldAttributeName = "Test.OldAttribute";
            var oldAttribute = new Mock<IAttributeData>();
            oldAttribute.SetupGet(x => x.Name).Returns(oldAttributeName);

            var oldField = new Mock<IMethodData>();
            oldField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    oldAttribute.Object
                });

            var newField = new Mock<IMethodData>();
            newField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>());

            var SUT = new MethodDiff(oldField.Object, newField.Object);
            Assert.True(SUT.RemovedAttributes.Any(x => x.Name == oldAttributeName));
            Assert.False(SUT.AddedAttributes.Any());
        }

        [Test]
        public void HasChanged_IsNotSet_ForUnChangedField()
        {
            var unchangedAttributeName = "Test.SameAttribute";
            var unchangedAttribute = new Mock<IAttributeData>();
            unchangedAttribute.SetupGet(x => x.Name).Returns(unchangedAttributeName);

            var oldField = new Mock<IMethodData>();
            oldField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    unchangedAttribute.Object
                });

            var newField = new Mock<IMethodData>();
            newField.SetupGet(x => x.AttributeData)
                .Returns(new List<IAttributeData>()
                {
                    unchangedAttribute.Object
                });

            var SUT = new MethodDiff(oldField.Object, newField.Object);
            Assert.False(SUT.HasChanged);
        }
	}
}
