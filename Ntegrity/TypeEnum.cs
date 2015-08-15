namespace Ntegrity
{
	public enum TypeEnum
	{
		Class,
		Interface,
		Enum,
		Struct
	}

	public static class TypeEnumHelpers
	{
		public static string GetKeywordFromEnum(TypeEnum type)
		{
			switch (type)
			{
				case TypeEnum.Class:
					return "class";
				case TypeEnum.Struct:
					return "struct";
				case TypeEnum.Enum:
					return "enum";
				case TypeEnum.Interface:
					return "interface";
			}
			throw new NtegrityException("Unable to determine keyword for TypeEnum");
		}
	}
}