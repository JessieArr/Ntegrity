using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntegrity.TestTargetAssembly
{
    public class PropertyTestClass
    {
        public int PublicProperty { get; set; }
        internal int InternalProperty { get; set; }
        protected int ProtectedProperty { get; set; }
        private int PrivateProperty { get; set; }
    }
}
