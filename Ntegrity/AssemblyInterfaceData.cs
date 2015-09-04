using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ntegrity
{
	public class AssemblyInterfaceData
	{
		public readonly List<TypeInterfaceData> Types = new List<TypeInterfaceData>();
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
                Types.Add(new TypeInterfaceData(type));
            }
        }

        public string GenerateHumanReadableInterfaceDefinition()
        {
            return GenerateHumanReadableInterfaceDefinition(new NtegrityOutputSettings());
        }

        public string GenerateHumanReadableInterfaceDefinition(NtegrityOutputSettings settings)
        {
            var returnString = "This is a human readable representation of the interface for the assembly:" + Environment.NewLine;

            returnString += "Assembly: " + Name + Environment.NewLine;
            returnString += "Version:  " + Version + Environment.NewLine;
            returnString += "Targeting CLR version: " + CLRVersion + Environment.NewLine + Environment.NewLine;

            returnString += "Referenced Assemblies:" + Environment.NewLine;
            foreach (var assembly in ReferencedAssemblies)
            {
                returnString += assembly + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var classes = Types.Where(x => x.Type == TypeEnum.Class);

            returnString += "CLASSES: " + Environment.NewLine;
            foreach (var classType in classes)
            {
                if (!classType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += classType.ToString("\t") + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var interfaces = Types.Where(x => x.Type == TypeEnum.Interface);
            returnString += "INTERFACES: " + Environment.NewLine;
            foreach (var interfaceType in interfaces)
            {
                if (!interfaceType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += interfaceType.ToString("\t") + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var enums = Types.Where(x => x.Type == TypeEnum.Enum);
            returnString += "ENUMS: " + Environment.NewLine;
            foreach (var enumType in enums)
            {
                if (!enumType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += enumType.ToString("\t") + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var structs = Types.Where(x => x.Type == TypeEnum.Struct);
            returnString += "STRUCTS: " + Environment.NewLine;
            foreach (var structType in structs)
            {
                if (!structType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += structType.ToString("\t") + Environment.NewLine;
            }

            return returnString;
        }
    }

    public class NtegrityOutputSettings
    {
        public AccessLevelEnum ShowTypesAtOrAboveAccessLevel { get; set; }

        public NtegrityOutputSettings()
        {
            ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Private;
        }
    }
}
