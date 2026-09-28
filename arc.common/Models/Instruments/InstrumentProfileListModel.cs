namespace arc.common.Models.Instruments
{
    public class InstrumentProfileListModel
    {
        public string Id { get; set; }
        public string InstrumentName { get; set; }
        /// <summary>Resolved display text for the InstrumentMachine list item (vendor/platform machine).</summary>
        public string InstrumentMachine { get; set; }
        public string SpecimenType { get; set; }
        public string CultureType { get; set; }
        public string DirectTest { get; set; }
        public string CultureTest { get; set; }
        public string OrganismGroup { get; set; }
    }
}
