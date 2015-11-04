using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ntegrity.Models;
using Ntegrity.Models.Diff;

namespace Ntegrity
{
    public class NtegrityAssemblyDiff
    {
        public AssemblyInterfaceData OldAssembly;
        public AssemblyInterfaceData NewAssembly;

        public List<ClassTypeData> AddedClasses = new List<ClassTypeData>();
        public List<ClassTypeData> RemovedClasses = new List<ClassTypeData>();

        public List<InterfaceTypeData> AddedInterfaces = new List<InterfaceTypeData>();
        public List<InterfaceTypeData> RemovedInterfaces = new List<InterfaceTypeData>();

        public List<StructTypeData> AddedStructs = new List<StructTypeData>();
        public List<StructTypeData> RemovedStructs = new List<StructTypeData>();

        public List<EnumTypeData> AddedEnums = new List<EnumTypeData>();
        public List<EnumTypeData> RemovedEnums = new List<EnumTypeData>();
        public List<EnumTypeDiff> ModifiedEnums = new List<EnumTypeDiff>();

        public NtegrityAssemblyDiff(AssemblyInterfaceData oldAssembly, AssemblyInterfaceData newAssembly)
        {
            OldAssembly = oldAssembly;
            NewAssembly = newAssembly;
            GetAddedAndRemovedClasses(oldAssembly, newAssembly);
            GetAddedAndRemovedInterfaces(oldAssembly, newAssembly);
            GetAddedAndRemovedStructs(oldAssembly, newAssembly);
            GetAddedAndRemovedEnums(oldAssembly, newAssembly);
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

        private void GetAddedAndRemovedInterfaces(AssemblyInterfaceData oldAssembly, AssemblyInterfaceData newAssembly)
        {
            foreach (var oldInterface in oldAssembly.Interfaces)
            {
                if (newAssembly.Interfaces.All(x => x.Name != oldInterface.Name))
                {
                    RemovedInterfaces.Add(oldInterface);
                }
            }
            foreach (var newInterface in newAssembly.Interfaces)
            {
                if (oldAssembly.Interfaces.All(x => x.Name != newInterface.Name))
                {
                    AddedInterfaces.Add(newInterface);
                }
            }
        }

        private void GetAddedAndRemovedStructs(AssemblyInterfaceData oldAssembly, AssemblyInterfaceData newAssembly)
        {
            foreach (var oldStruct in oldAssembly.Structs)
            {
                if (newAssembly.Structs.All(x => x.Name != oldStruct.Name))
                {
                    RemovedStructs.Add(oldStruct);
                }
            }
            foreach (var oldStruct in newAssembly.Structs)
            {
                if (oldAssembly.Structs.All(x => x.Name != oldStruct.Name))
                {
                    AddedStructs.Add(oldStruct);
                }
            }
        }

        private void GetAddedAndRemovedEnums(AssemblyInterfaceData oldAssembly, AssemblyInterfaceData newAssembly)
        {
            foreach (var oldEnum in oldAssembly.Enums)
            {
                if (newAssembly.Enums.All(x => x.Name != oldEnum.Name))
                {
                    RemovedEnums.Add(oldEnum);
                }
            }
            foreach (var newEnum in newAssembly.Enums)
            {
                if (oldAssembly.Enums.All(x => x.Name != newEnum.Name))
                {
                    AddedEnums.Add(newEnum);
                }
            }
            foreach (var oldModifiedEnum in oldAssembly.Enums)
            {
                var newModifiedEnum = newAssembly.Enums.FirstOrDefault(
                    x => x.Name == oldModifiedEnum.Name);
                if (newModifiedEnum == null)
                {
                    continue;
                }
                ModifiedEnums.Add(new EnumTypeDiff(oldModifiedEnum, newModifiedEnum));
            }
        }

        public override string ToString()
        {
            return ToString(new NtegrityOutputSettings());
        }

        private const string FromAssemblyPrefix = "Diff of Upgrade from Assembly: ";
        private const string ToAssemblyPrefix = "To Assembly: ";
        private const string RemovedClassesPrefix = "REMOVED CLASSES:";
        private const string AddedClassesPrefix = "ADDED CLASSES:";
        private const string RemovedInterfacesPrefix = "REMOVED INTERFACES:";
        private const string AddedInterfacesPrefix = "ADDED INTERFACES:";
        private const string RemovedStructsPrefix = "REMOVED STRUCTS:";
        private const string AddedStructsPrefix = "ADDED STRUCTS:";
        private const string RemovedEnumsPrefix = "REMOVED ENUMS:";
        private const string AddedEnumsPrefix = "ADDED ENUMS:";
        private const string ChangedEnumsPrefix = "CHANGED ENUMS:";
        public string ToString(NtegrityOutputSettings outputSettings)
        {
            var returnString = FromAssemblyPrefix + OldAssembly.Name + " " + OldAssembly.Version;
            returnString += Environment.NewLine;
            returnString += ToAssemblyPrefix + NewAssembly.Name + " " + NewAssembly.Version;
            returnString += Environment.NewLine + Environment.NewLine;

            // CLASSES
            returnString += RemovedClassesPrefix + Environment.NewLine;
            foreach (var removedClass in RemovedClasses)
            {
                returnString += "-\t" + removedClass.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            returnString += AddedClassesPrefix + Environment.NewLine;
            foreach (var addedClass in AddedClasses)
            {
                returnString += "+\t" + addedClass.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            // INTERFACES
            returnString += RemovedInterfacesPrefix + Environment.NewLine;
            foreach (var removedInterface in RemovedInterfaces)
            {
                returnString += "-\t" + removedInterface.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            returnString += AddedInterfacesPrefix + Environment.NewLine;
            foreach (var addedInterface in AddedInterfaces)
            {
                returnString += "+\t" + addedInterface.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            // STRUCTS
            returnString += RemovedStructsPrefix + Environment.NewLine;
            foreach (var removedStruct in RemovedStructs)
            {
                returnString += "-\t" + removedStruct.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            returnString += AddedStructsPrefix + Environment.NewLine;
            foreach (var addedStruct in AddedStructs)
            {
                returnString += "+\t" + addedStruct.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            // ENUMS
            returnString += RemovedEnumsPrefix + Environment.NewLine;
            foreach (var removedEnum in RemovedEnums)
            {
                returnString += "-\t" + removedEnum.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            returnString += AddedEnumsPrefix + Environment.NewLine;
            foreach (var addedEnum in AddedEnums)
            {
                returnString += "+\t" + addedEnum.Name + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var changedEnums = ModifiedEnums.Where(x => x.HasChanged);
            returnString += ChangedEnumsPrefix + Environment.NewLine;
            foreach (var changedEnum in changedEnums)
            {
                returnString += "+\t" + changedEnum.ToString() + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            return returnString;
        }
    }
}
