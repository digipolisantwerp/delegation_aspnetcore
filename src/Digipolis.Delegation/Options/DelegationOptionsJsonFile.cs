namespace Digipolis.Delegation.Options
{
    public class DelegationOptionsJsonFile
    {
        public DelegationOptionsJsonFile() : this(DelegationOptionsDefaults.OptionsFileName, DelegationOptionsDefaults.OptionsFileAuthDelegationSection)
        { }

        public DelegationOptionsJsonFile(string fileName, string section)
        {
            FileName = fileName;
            Section = section;
        }

        public string BasePath { get; set; }
        public string FileName { get; set; }
        public string Section { get; set; }
    }
}
