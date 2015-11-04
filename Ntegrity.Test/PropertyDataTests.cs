using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.Models.Reflection;
using Ntegrity.TestTargetAssembly;
using NUnit.Framework;

namespace Ntegrity.Test
{
    [TestFixture]
    public class PropertyDataTests
    {
        [Test]
        public void PublicPropertyAccessLevel_IsPublic()
        {
            var property = new PropertyInfoWrapper(typeof(ContainerClass).GetProperties().Single(x => x.Name == "PublicPropertyWithAttributes"));
            var SUT = new PropertyData(property);
            Assert.That(SUT.GetterAccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PropertyWithAttributes_HasAttributes()
        {
            var property = new PropertyInfoWrapper(typeof(ContainerClass).GetProperties().Single(x => x.Name == "PublicPropertyWithAttributes"));
            var SUT = new PropertyData(property);
            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }

        [Test]
        public void PublicPropertyDataFromString_IsPublic()
        {
            var testString = "\t\tVoid TestField { public get; public set; }";
            var propertyData = new PropertyData(testString);
            Assert.That(propertyData.GetterAccessLevel == AccessLevelEnum.Public);
        }

        [Test]
        public void PrivatePropertyDataFromString_IsPrivate()
        {
            var testString = "\t\tVoid TestField { private get; private set; }";
            var propertyData = new PropertyData(testString);
            Assert.That(propertyData.GetterAccessLevel == AccessLevelEnum.Private);
        }

        [Test]
        public void ProtectedPropertyDataFromString_IsProtected()
        {
            var testString = "\t\tVoid TestField { protected get; protected set; }";
            var propertyData = new PropertyData(testString);
            Assert.That(propertyData.GetterAccessLevel == AccessLevelEnum.Protected);
        }

        [Test]
        public void InternalPropertyDataFromString_IsInternal()
        {
            var testString = "\t\tVoid TestField { internal get; internal set; }";
            var propertyData = new PropertyData(testString);
            Assert.That(propertyData.GetterAccessLevel == AccessLevelEnum.Internal);
        }

        [Test]
        public void PropertyWithAttributesFromString_HasAttributes()
        {
            var property = new PropertyInfoWrapper(typeof(ContainerClass).GetProperties().Single(x => x.Name == "PublicPropertyWithAttributes"));
            var testPropertyData = new PropertyData(property);

            var testString = testPropertyData.ToString();
            var SUT = new PropertyData(testString);

            Assert.That(SUT.AttributeData.Count > 0);
            Assert.That(SUT.AttributeData.Any(x => x.Name == typeof(TestAttributeAttribute).FullName));
        }
    }
}
