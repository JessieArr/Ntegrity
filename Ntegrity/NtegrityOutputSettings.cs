using Ntegrity.Models;

namespace Ntegrity
{
    public class NtegrityOutputSettings
    {
        public AccessLevelEnum ShowTypesAtOrAboveAccessLevel { get; set; }

        public string TypePrefix { get; set; }
        public string MemberPrefix { get; set; }

        public bool ShowCompilerAttributes { get; set; }

        public NtegrityOutputSettings()
        {
            ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Private;

            TypePrefix = "\t";
            MemberPrefix = "\t\t";

            ShowCompilerAttributes = false;
        }
    }
}