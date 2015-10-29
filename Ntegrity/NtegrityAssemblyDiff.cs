using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;

namespace Ntegrity
{
    public class NtegrityAssemblyDiff
    {
        public List<ClassTypeData> AddedClasses = new List<ClassTypeData>();
        public List<ClassTypeData> RemovedClasses = new List<ClassTypeData>();

        public NtegrityAssemblyDiff(AssemblyInterfaceData oldAssembly, AssemblyInterfaceData newAssembly)
        {
            
        }

        private void GetAddedAndRemovedClasses(AssemblyInterfaceData oldAssembly, AssemblyInterfaceData newAssembly)
        {
            foreach (var oldClass in oldAssembly.Classes)
            {
                if (newAssembly.Classes.All(x => x.Name != oldClass.Name))
                {
                    RemovedClasses.Add(oldClass);
                }
            }
            foreach (var newClass in newAssembly.Classes)
            {
                if (oldAssembly.Classes.All(x => x.Name != newClass.Name))
                {
                    AddedClasses.Add(newClass);
                }
            }
        }
    }
}
