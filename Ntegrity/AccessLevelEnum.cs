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

	    public static bool HasAvailabilityEqualToOrGreaterThan(this AccessLevelEnum compared, AccessLevelEnum comparedTo)
	    {
	        if (comparedTo == AccessLevelEnum.Private)
	        {
	            return true;
	        }
	        if (comparedTo == AccessLevelEnum.Protected)
	        {
	            if (compared == AccessLevelEnum.Public
	                || compared == AccessLevelEnum.Internal
	                || compared == AccessLevelEnum.Protected)
	            {
	                return true;
	            }
	        }
            if (comparedTo == AccessLevelEnum.Internal)
            {
                if (compared == AccessLevelEnum.Public
                    || compared == AccessLevelEnum.Internal)
                {
                    return true;
                }
            }
            if (comparedTo == AccessLevelEnum.Public)
            {
                if (compared == AccessLevelEnum.Public)
                {
                    return true;
                }
            }

            return false;
	    }
	}
}