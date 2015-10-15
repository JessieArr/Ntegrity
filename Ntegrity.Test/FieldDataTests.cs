using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
    [TestFixture]
    public class FieldDataTests
    {
        [Test]
        public void PublicFieldAccessLevel_IsPublic()
        {
            var field = typeof(ContainerClass).GetFields().Single(x => x.Name == "PublicFieldWithAttributes");
            var SUT = new FieldData(field);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void FieldWithAttributes_HasAttributes()
        {
            var field = typeof(ContainerClass).GetFields().Single(x => x.Name == "PublicFieldWithAttributes");
            var SUT = new FieldData(field);
            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }

        [Test]
        public void PublicFieldDataFromString_IsPublic()
        {
            var testString = "\t\tpublic Void TestField";
            var fieldData = new FieldData(testString);
            Assert.That(fieldData.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivateFieldDataFromString_IsPrivate()
        {
            var testString = "\t\tprivate Void TestField";
            var fieldData = new FieldData(testString);
            Assert.That(fieldData.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ProtectedFieldDataFromString_IsProtected()
        {
            var testString = "\t\tprotected Void TestField";
            var fieldData = new FieldData(testString);
            Assert.That(fieldData.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void InternalFieldDataFromString_IsInternal()
        {
            var testString = "\t\tinternal Void TestField";
            var fieldData = new FieldData(testString);
            Assert.That(fieldData.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void FieldWithAttributesFromString_HasAttributes()
        {
            var field = typeof(ContainerClass).GetFields().Single(x => x.Name == "PublicFieldWithAttributes");
            var testFieldData = new FieldData(field);

            var testString = testFieldData.ToString();
            var SUT = new FieldData(testString);

            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }
    }
}
