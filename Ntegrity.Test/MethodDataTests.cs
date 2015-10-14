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
    }
}
