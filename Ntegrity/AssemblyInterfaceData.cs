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
		public readonly string Name;
		public readonly string Version;
		public readonly string CLRVersion;

		public AssemblyInterfaceData(Type typeInAssemblyToAnalyze)
		{
			Assembly = typeInAssemblyToAnalyze.Assembly;

			Name = Assembly.GetName().Name;
			Version = "v" + Assembly.GetName().Version;
			CLRVersion = Assembly.ImageRuntimeVersion;

			var types = Assembly.GetTypes();

			foreach (var type in types)
			{
				Types.Add(new TypeInterfaceData(type));
			}
		}

		public string GenerateHumanReadableInterfaceDefinition()
		{
			var returnString = "This is a human readable representation of the interface for the assembly:" + Environment.NewLine;

            returnString += "Assembly: " + Name + Environment.NewLine;
			returnString += "Version:  " + Version + Environment.NewLine;
            returnString += "Targeting CLR version: " + CLRVersion + Environment.NewLine + Environment.NewLine;

			var classes = Types.Where(x => x.Type == TypeEnum.Class);

			returnString += "CLASSES: " + Environment.NewLine;
			foreach (var classType in classes)
			{
				returnString += classType + Environment.NewLine;
			}
			returnString += Environment.NewLine;

			var interfaces = Types.Where(x => x.Type == TypeEnum.Interface);
			returnString += "INTERFACES: " + Environment.NewLine;
			foreach (var interfaceType in interfaces)
			{
				returnString += interfaceType + Environment.NewLine;
			}
			returnString += Environment.NewLine;

			var enums = Types.Where(x => x.Type == TypeEnum.Enum);
			returnString += "ENUMS: " + Environment.NewLine;
			foreach (var enumType in enums)
			{
				returnString += enumType + Environment.NewLine;
			}
			returnString += Environment.NewLine;

			var structs = Types.Where(x => x.Type == TypeEnum.Struct);
			returnString += "STRUCTS: " + Environment.NewLine;
			foreach (var structType in structs)
			{
				returnString += structType + Environment.NewLine;
			}

			return returnString;
		}
	}
}
