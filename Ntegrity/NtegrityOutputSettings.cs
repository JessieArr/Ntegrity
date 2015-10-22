using Ntegrity.Models;

namespace Ntegrity
{
    public class NtegrityOutputSettings
    {
        public AccessLevelEnum ShowTypesAtOrAboveAccessLevel { get; set; }

        public string TypePrefix { get; set; }
        public string MemberPrefix { get; set; }

        // Attributes
        public bool ShowCompilerAttributes { get; set; }

        // Methods
        public bool ShowInheritedMethods { get; set; }
        public bool ShowMethodsInheritedFromSystemTypes { get; set; }

        public NtegrityOutputSettings()
        {
            ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Private;

            TypePrefix = "\t";
            MemberPrefix = "\t\t";

            ShowCompilerAttributes = false;
            ShowInheritedMethods = true;
            ShowMethodsInheritedFromSystemTypes = false;
        }
    }
}