using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.Models.Reflection;
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
            var constructor = new ConstructorInfoWrapper(typeof(PublicClass).GetConstructors().First());
            var SUT = new ConstructorData(constructor);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivateConstructorAccessLevel_IsPublic()
        {
            var constructor = new ConstructorInfoWrapper(typeof(PrivateAttributeConstructorTestClass).GetConstructors(BindingFlags.NonPublic|BindingFlags.Instance).First());
            var SUT = new ConstructorData(constructor);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ConstructorWithAttributes_HasAttributes()
        {
            var constructor = new ConstructorInfoWrapper(typeof(PublicAttributeConstructorTestClass).GetConstructors().First());
            var SUT = new ConstructorData(constructor);
            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }

        [Test]
        public void PublicConstructorDataFromString_IsPublic()
        {
            var testString = "\t\tpublic Void .ctor()";
            var constructorData = new ConstructorData(testString);
            Assert.That(constructorData.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivateConstructorDataFromString_IsPrivate()
        {
            var testString = "\t\tprivate Void .ctor()";
            var constructorData = new ConstructorData(testString);
            Assert.That(constructorData.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ProtectedConstructorDataFromString_IsProtected()
        {
            var testString = "\t\tprotected Void .ctor()";
            var constructorData = new ConstructorData(testString);
            Assert.That(constructorData.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void InternalConstructorDataFromString_IsInternal()
        {
            var testString = "\t\tinternal Void .ctor()";
            var constructorData = new ConstructorData(testString);
            Assert.That(constructorData.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void ConstructorWithAttributesFromString_HasAttributes()
        {
            var constructor = new ConstructorInfoWrapper(typeof(PublicAttributeConstructorTestClass).GetConstructors().First());
            var testConstructorData = new ConstructorData(constructor);

            var testString = testConstructorData.ToString();
            var SUT = new ConstructorData(testString);

            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }
    }
}
