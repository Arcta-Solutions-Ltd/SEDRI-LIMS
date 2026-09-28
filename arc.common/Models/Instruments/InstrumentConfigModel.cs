namespace arc.common.Models.Instruments
{
    public class InstrumentConfigModel
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Channel { get; set; }
        public string Enabled { get; set; }
        public string ApprovalRequired { get; set; }
        public string SupportedTests { get; set; }
        public string EnabledTests { get; set; }
    }
}
