namespace arc.common.Models.Instruments
{
    public class InstrumentResultsAddModel
    {
        public int CultureId { get; set; }
        public string TestType { get; set; }
        public string InstrumentName { get; set; }
        public string ManufacturersBarcode { get; set; }
        public string Status { get; set; }
        public string SpecimenQuantity { get; set; }
        public string CompletionDateTime { get; set; }
        public string RawData { get; set; }
    }
}
