namespace arc.common.Models.Instruments
{
    public class InstrumentCommonModel
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public string TestType { get; set; }
        public string ManufacturersBarcode { get; set; }
        public string Status { get; set; }
        public string SpecimenQuantity { get; set; }
        public string CompletionDateTime { get; set; }
    }
}
