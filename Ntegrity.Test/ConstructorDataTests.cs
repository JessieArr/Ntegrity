using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
    [TestFixture]
    public class ConstructorDataTests
    {
        [Test]
        public void PublicConstructorAccessLevel_IsPublic()
        {
            var field = typeof(PublicClass).GetConstructors().First();
            var SUT = new ConstructorData(field);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivateConstructorAccessLevel_IsPublic()
        {
            var field = typeof(PrivateAttributeConstructorTestClass).GetConstructors(BindingFlags.NonPublic|BindingFlags.Instance).First();
            var SUT = new ConstructorData(field);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ConstructorWithAttributes_HasAttributes()
        {
            var method = typeof(PublicAttributeConstructorTestClass).GetConstructors().First();
            var SUT = new ConstructorData(method);
            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }

        [Test]
        public void PublicConstructorDataFromString_IsPublic()
        {
            var testString = "\t\tpublic Void .ctor()";
            var methodData = new ConstructorData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivateConstructorDataFromString_IsPrivate()
        {
            var testString = "\t\tprivate Void .ctor()";
            var methodData = new ConstructorData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ProtectedConstructorDataFromString_IsProtected()
        {
            var testString = "\t\tprotected Void .ctor()";
            var methodData = new ConstructorData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void InternalConstructorDataFromString_IsInternal()
        {
            var testString = "\t\tinternal Void .ctor()";
            var methodData = new ConstructorData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void ConstructorWithAttributesFromString_HasAttributes()
        {
            var attributeName = "Ntegrity.TestTargetAssembly.TestAttributeAttribute";
            var field = typeof(PublicAttributeConstructorTestClass).GetConstructors().First();
            var testFieldData = new ConstructorData(field);

            var testString = testFieldData.ToString();
            var SUT = new ConstructorData(testString);

            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == attributeName));
        }
    }
}
