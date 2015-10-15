using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntegrity.TestTargetAssembly
{
    public class PublicAttributeConstructorTestClass
    {
        [TestAttribute("Test")]
        public PublicAttributeConstructorTestClass()
        {

        }
    }

    public class InternalAttributeConstructorTestClass
    {
        [TestAttribute("Test")]
        internal InternalAttributeConstructorTestClass()
        {

        }
    }

    public class ProtectedAttributeConstructorTestClass
    {
        [TestAttribute("Test")]
        protected ProtectedAttributeConstructorTestClass()
        {

        }
    }

    public class PrivateAttributeConstructorTestClass
    {
        [TestAttribute("Test")]
        private PrivateAttributeConstructorTestClass()
        {

        }
    }
}
