using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntegrity.TestTargetAssembly
{
    public class ContainerClass
    {
        #region Nested Private
        private class NestedPrivateClass
        {

        }

        private interface INestedPrivateInterface
        {

        }

        private enum NestedPrivateEnum
        {

        }

        private struct NestedPrivateStruct
        {

        }
        #endregion

        #region Nested Protected
        protected class NestedProtectedClass
        {

        }

        protected interface INestedProtectedInterface
        {

        }

        protected enum NestedProtectedEnum
        {

        }

        protected struct NestedProtectedStruct
        {

        }
        #endregion

        #region Nested Internal
        internal class NestedInternalClass
        {

        }

        internal interface INestedInternalInterface
        {

        }

        internal enum NestedInternalEnum
        {

        }

        internal struct NestedInternalStruct
        {

        }
        #endregion

        #region Nested Public
        public class NestedPublicClass
        {
            public string test;
        }

        public interface INestedPublicInterface
        {

        }

        public enum NestedPublicEnum
        {

        }

        public struct NestedPublicStruct
        {

        }
        #endregion

        #region Members With Attributes

        [TestAttribute("Sample Attribute Text")]
        public void PublicMethodWithAttributes()
        {
        }

        [TestAttribute("Sample Attribute Text")]
        public int PublicPropertyWithAttributes { get; set; }

        [TestAttribute("Sample Attribute Text")]
        public int PublicFieldWithAttributes;

        // PublicConstructorWithAttributes
        [TestAttribute("Sample Attribute Text")]
        public ContainerClass()
        {
        }

        #endregion
    }
}
