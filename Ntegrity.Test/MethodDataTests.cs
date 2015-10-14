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
    public class MethodDataTests
    {
        [Test]
        public void PublicMethodAccessLevel_IsPublic()
        {
            var method = typeof(ContainerClass).GetMethods().Single(x => x.Name == "PublicMethodWithAttributes");
            var SUT = new MethodData(method);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void MethodWithAttributes_HasAttributes()
        {
            var method = typeof(ContainerClass).GetMethods().Single(x => x.Name == "PublicMethodWithAttributes");
            var SUT = new MethodData(method);
            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }

        [Test]
        public void PublicMethodDataFromString_IsPublic()
        {
            var testString = "\t\tpublic Void TestMethod()";
            var methodData = new MethodData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivateMethodDataFromString_IsPrivate()
        {
            var testString = "\t\tprivate Void TestMethod()";
            var methodData = new MethodData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ProtectedMethodDataFromString_IsProtected()
        {
            var testString = "\t\tprotected Void TestMethod()";
            var methodData = new MethodData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void InternalMethodDataFromString_IsInternal()
        {
            var testString = "\t\tinternal Void TestMethod()";
            var methodData = new MethodData(testString);
            Assert.That(methodData.AccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void MethodWithAttributesFromString_HasAttributes()
        {
            var attributeName = "Ntegrity.TestTargetAssembly.TestAttributeAttribute";
            var method = typeof(ContainerClass).GetMethods().Single(x => x.Name == "PublicMethodWithAttributes");
            var testMethodData = new MethodData(method);

            var testString = testMethodData.ToString();
            var SUT = new MethodData(testString);

            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == attributeName));
        }
    }
}
