namespace arc.common.Models.Instruments
{
    public class InstrumentRequestModel
    {
        public int Id { get; set; }
        public string InstrumentName { get; set; }
        public int SpecimenId { get; set; }
        public int CultureId { get; set; }
        public string AccessionNumber { get; set; }
        public string PatientRef { get; set; }
        public string Barcode {get; set; }
        public string CultureNumber { get; set; }
        public string OrganismCode { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        /// <summary>
        /// InstrumentMachine list item id when set on the <c>instrumentresults</c> row (list 144).
        /// </summary>
        public int? InstrumentMachineId { get; set; }

        /// <summary>
        /// Specimen collection date/time for Vitek 2 <c>collectedDateTime</c> (14 digits: YYYYMMDDHHMMSS).
        /// </summary>
        public string CollectedDateTime { get; set; } = "00000000000000";

        /// <summary>
        /// Specimen type list display name for the specimen (Vitek 2 outbound).
        /// </summary>
        public string SpecimenTypeName { get; set; } = "";

        /// <summary>
        /// JSON object string for the Bla offline test element, or "{}" for an empty <c>&lt;test&gt;&lt;/test&gt;</c> (Vitek 2 outbound).
        /// </summary>
        public string BlaTestJson { get; set; } = "{}";
    }
}
