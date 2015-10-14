﻿using System;
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

        public AssemblyInterfaceData(string humanReadableAssemblyInterface)
        {
            var lines = humanReadableAssemblyInterface.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            var assemblyLine = lines[1];
            var versionLine = lines[2];
            var clrVersionLine = lines[3];
            Name = assemblyLine.Substring(AssemblyNamePrefix.Length);
            Version = versionLine.Substring(AssemblyVersionPrefix.Length);
            CLRVersion = clrVersionLine.Substring(CLRVersionPrefix.Length);

            var i = 4;
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
            var returnString = "This is a human readable representation of the interface for the assembly:" + Environment.NewLine;

            returnString += AssemblyNamePrefix + Name + Environment.NewLine;
            returnString += AssemblyVersionPrefix + Version + Environment.NewLine;
            returnString += CLRVersionPrefix + CLRVersion + Environment.NewLine + Environment.NewLine;

            returnString += ReferencedAssembliesPrefix + Environment.NewLine;
            foreach (var assembly in ReferencedAssemblies)
            {
                returnString += assembly + Environment.NewLine;
            }
            returnString += Environment.NewLine;

            var classes = Types.Where(x => x.Type == TypeEnum.Class);

            returnString += "CLASSES:" + Environment.NewLine;
            foreach (var classType in classes)
            {
                if (!classType.AccessLevel.HasAvailabilityEqualToOrGreaterThan(
                    settings.ShowTypesAtOrAboveAccessLevel))
                {
                    continue;
                }
                returnString += classType.ToString(settings.SpacingString) + Environment.NewLine;
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
                returnString += interfaceType.ToString(settings.SpacingString) + Environment.NewLine;
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
                returnString += enumType.ToString(settings.SpacingString) + Environment.NewLine;
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
                returnString += structType.ToString(settings.SpacingString) + Environment.NewLine;
            }

            return returnString;
        }
    }
}
