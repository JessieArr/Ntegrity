namespace Ntegrity
{
	public enum AccessLevelEnum
	{
		Private,
		Protected,
		Internal,
		Public
	}

	public static class AccessLevelEnumHelpers
	{
		public static string GetKeywordFromEnum(this AccessLevelEnum input)
		{
			switch (input)
			{
				case AccessLevelEnum.Public:
					return "public";
				case AccessLevelEnum.Internal:
					return "internal";
				case AccessLevelEnum.Protected:
					return "protected";
				case AccessLevelEnum.Private:
					return "private";
			}
			throw new NtegrityException("Unable to determine keyword for AccessLevelEnum");
		}
	}
}