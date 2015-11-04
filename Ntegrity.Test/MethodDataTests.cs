using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.Models.Reflection;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;
using Moq;

namespace Ntegrity.Test
{
    [TestFixture]
    public class MethodDataTests
    {
        [Test]
        public void PublicMethodAccessLevel_IsPublic()
        {
            var method = new MethodInfoWrapper(typeof(ContainerClass).GetMethods().Single(x => x.Name == "PublicMethodWithAttributes"));
            var SUT = new MethodData(method);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void InheritedMethod_IsIdentified()
        {
            var method = new MethodInfoWrapper(typeof(ContainerClass).GetMethods().Single(x => x.Name == "ToString"));
            var SUT = new MethodData(method);
            Assert.That(SUT.AccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void MethodWithAttributes_HasAttributes()
        {
            var method = new MethodInfoWrapper(typeof(ContainerClass).GetMethods().Single(x => x.Name == "PublicMethodWithAttributes"));
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
            var method = new MethodInfoWrapper(typeof(ContainerClass).GetMethods().Single(x => x.Name == "PublicMethodWithAttributes"));
            var testMethodData = new MethodData(method);

            var testString = testMethodData.ToString();
            var SUT = new MethodData(testString);

            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }

        [Test]
        public void MethodWithAttributes_SortsAttributes()
        {
            var testMethodSignature = "Int32 DeclaringType.Test()";
            var methodWrapper = new Mock<IMethodInfoWrapper>();
            methodWrapper.Setup(x => x.ToString())
                .Returns(testMethodSignature);
            methodWrapper.SetupGet(x => x.IsPublic)
                .Returns(true);
            var declaringType = new Mock<ITypeWrapper>();
            methodWrapper.SetupGet(x => x.DeclaringType)
                .Returns(declaringType.Object);
            var reflectedType = new Mock<ITypeWrapper>();
            methodWrapper.SetupGet(x => x.ReflectedType)
                .Returns(reflectedType.Object);

            var firstTestAttribute = new Mock<IAttributeWrapper>();
            firstTestAttribute.Setup(x => x.ToString())
                .Returns("ZZZ");
            var secondTestAttribute = new Mock<IAttributeWrapper>();
            secondTestAttribute.Setup(x => x.ToString())
                .Returns("AAA");
            methodWrapper.Setup(x => x.GetCustomAttributes())
                .Returns(new List<IAttributeWrapper>()
                {
                    firstTestAttribute.Object,
                    secondTestAttribute.Object,
                });

            var SUT = new MethodData(methodWrapper.Object);
            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.First().Name == "AAA");
            Assert.That(SUT.AttributeData.Last().Name == "ZZZ");
        }
    }
}
