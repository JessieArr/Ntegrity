using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ntegrity.Models
{
	public class AssemblyInterfaceData
	{
        public readonly List<ClassTypeData> Classes = new List<ClassTypeData>();
        public readonly List<InterfaceTypeData> Interfaces = new List<InterfaceTypeData>();
        public readonly List<EnumTypeData> Enums = new List<EnumTypeData>();
        public readonly List<StructTypeData> Structs = new List<StructTypeData>();
		public readonly Assembly Assembly;
	    public readonly List<string> ReferencedAssemblies; 
		public readonly string Name;
		public readonly string Version;
		public readonly string CLRVersion;

        public AssemblyInterfaceData(Type typeInAssemblyToAnalyze)
        {
            Assembly = typeInAssemblyToAnalyze.Assembly;

            Name = Assembly.GetName().Name;
            Version = "v" + Assembly.GetName().Version;
            CLRVersion = Assembly.ImageRuntimeVersion;

            ReferencedAssemblies = Assembly.GetReferencedAssemblies().Select(x => x.FullName).ToList();

            var types = Assembly.GetTypes();

            foreach (var type in types)
            {
                var typeEnumValue = GetTypeEnumValueForType(type);
                switch (typeEnumValue)
                {
                    case TypeEnum.Class:
                        Classes.Add(new ClassTypeData(type));
                        break;
                    case TypeEnum.Interface:
                        Interfaces.Add(new InterfaceTypeData(type));
                        break;
                    case TypeEnum.Enum:
                        Enums.Add(new EnumTypeData(type));
                        break;
                    case TypeEnum.Struct:
                        Structs.Add(new StructTypeData(type));
                        break;
                }
            }

            Classes = Classes.OrderBy(x => x.Name).ToList();
            Interfaces = Interfaces.OrderBy(x => x.Name).ToList();
            Enums = Enums.OrderBy(x => x.Name).ToList();
            Structs = Structs.OrderBy(x => x.Name).ToList();
        }

	    private TypeEnum GetTypeEnumValueForType(Type typeToAnalyze)
	    {
            if (typeToAnalyze.IsClass)
            {
                return TypeEnum.Class;
            }
            if (typeToAnalyze.IsInterface)
            {
                return TypeEnum.Interface;
            }

            // Structs are value types, but not enums. Enums are both.
            if (typeToAnalyze.IsEnum && typeToAnalyze.IsValueType)
            {
                return TypeEnum.Enum;
            }
            if (typeToAnalyze.IsValueType)
            {
                return TypeEnum.Struct;
            }

            throw new NtegrityException("Unable to determine data type for type: " + typeToAnalyze.AssemblyQualifiedName);
        }

        public AssemblyInterfaceData(string humanReadableAssemblyInterface)
        {
            var lines = humanReadableAssemblyInterface.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var assemblyLine = lines[0];
            var versionLine = lines[1];
            var clrVersionLine = lines[2];
            Name = assemblyLine.Substring(AssemblyNamePrefix.Length);
            Version = versionLine.Substring(AssemblyVersionPrefix.Length);
            CLRVersion = clrVersionLine.Substring(CLRVersionPrefix.Length);

            var i = 3;
            if (String.Equals(lines[i], ReferencedAssembliesPrefix))
            {
                i++;
                ReferencedAssemblies = new List<string>();
                while (!lines[i].Contains(ClassesPrefix))
                {
                    ReferencedAssemblies.Add(lines[i]);
                    i++;
                }
            }
        }

        public string GenerateHumanReadableInterfaceDefinition()
        {
            return GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings());
        }

        private const string AssemblyNamePrefix = "Assembly: ";
        private const string AssemblyVersionPrefix = "Version: ";
        private const string CLRVersionPrefix = "Targeting CLR Version: ";
        private const string ReferencedAssembliesPrefix = "Referenced Assemblies:";
        private const string ClassesPrefix = "CLASSES:";

        public string GenerateHumanReadableInterfaceDefinition(NtegrityOutputSettings settings)
        {
            var returnString = AssemblyNamePrefix + Name + Environment.NewLine;
            returnString += AssemblyVersionPrefix + Version + Environment.NewLine;
            returnString += CLRVersionPrefix + CLRVersion + Environment.NewLine + Environment.NewLine;

            returnString += ReferencedAssembliesPrefix + Environment.NewLine;
            var referencedAssemblies = ReferencedAssemblies.OrderBy(x => x);
            foreach (var assembly in referencedAssemblies)
            {
                returnString += assembly + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var classes = Classes.OrderBy(x => x.Name);
            returnString += "CLASSES:" + Environment.NewLine;
            foreach (var classType in classes)
            {
                if (!classType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += classType.ToString(settings) + Environment.NewLine;
            }

            var interfaces = Interfaces.OrderBy(x => x.Name);
            returnString += "INTERFACES: " + Environment.NewLine;
            foreach (var interfaceType in interfaces)
            {
                if (!interfaceType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += interfaceType.ToString() + Environment.NewLine;
            }

            var enums = Enums.OrderBy(x => x.Name);
            returnString += "ENUMS: " + Environment.NewLine;
            foreach (var enumType in enums)
            {
                if (!enumType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += enumType.ToString() + Environment.NewLine;
            }

            var structs = Structs.OrderBy(x => x.Name);
            returnString += "STRUCTS: " + Environment.NewLine;
            foreach (var structType in structs)
            {
                if (!structType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += structType.ToString() + Environment.NewLine;
            }

            return returnString;
        }
    }
}
