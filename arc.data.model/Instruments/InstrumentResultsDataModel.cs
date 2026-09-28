namespace arc.data.model.Instruments
{
    /// <summary>
    /// Represents the fields in the instrumentresults table in the database.
    /// </summary>
    public class InstrumentResultsDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the instrument profile.
        /// </summary>
        public string InstrumentProfile { get; set; } = "";
        /// <summary>
        /// Gets or sets the specimen id that the result belongs to.
        /// </summary>
        public int SpecimenId { get; set; }
        /// <summary>
        /// Gets or sets the culture id that the result belongs to.
        /// </summary>
        public int CultureId { get; set; }
        /// <summary>
        /// Gets or sets the barcode used to identify the instrument result.
        /// </summary>
        public string? Barcode { get; set; }
        /// <summary>
        /// Gets or sets the date and time when the request was made.
        /// </summary>
        public DateTime RequestMade { get; set; }
        /// <summary>
        /// Gets or sets the date and time when the results were received from the instrument.
        /// </summary>
        public DateTime ResultReceived { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the instrumentresults table. This links to the instrument status list in the listitem table.
        /// </summary>
        public int StatusId { get; set; }
        /// <summary>
        /// Optional InstrumentMachine list item id (list 144), aligned with the instrument profile configuration.
        /// </summary>
        public int? InstrumentMachineId { get; set; }
        /// <summary>
        /// Copy of specimen accession at insert time (inbound matching).
        /// </summary>
        public string? AccessionNumber { get; set; }
        /// <summary>
        /// Copy of culture number when <see cref="CultureId"/> is set (inbound matching).
        /// </summary>
        public string? CultureNumber { get; set; }
        /// <summary>
        /// Gets or sets the contents of the MoreData field which contains any extra required data related to an instrument result in Json format.
        /// </summary>
        [Jsonb]
        public string MoreData { get; set; } = "{}";
        /// <summary>
        /// Gets or sets the contents of the record passed though the interface from the external system or machine.
        /// </summary>
        [Jsonb]
        public string RawResult { get; set; } = "{}";
    }
}

