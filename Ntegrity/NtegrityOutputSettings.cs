namespace Ntegrity
{
    public class NtegrityOutputSettings
    {
        public AccessLevelEnum ShowTypesAtOrAboveAccessLevel { get; set; }
        public string SpacingString { get; set; }

        public NtegrityOutputSettings()
        {
            ShowTypesAtOrAboveAccessLevel = AccessLevelEnum.Private;
            SpacingString = "\t";
        }
    }
}