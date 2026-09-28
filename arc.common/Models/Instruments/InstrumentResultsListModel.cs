namespace arc.common.Models.Instruments
{
    /// <summary>
    /// Represents the model for instrument results list.
    /// </summary>
    public class InstrumentResultsListModel
    {
        /// <summary>
        /// Gets or sets the identifier of the instrument result.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the instrument profile.
        /// </summary>
        public string InstrumentProfile { get; set; }

        /// <summary>
        /// Gets or sets the accession number.
        /// </summary>
        public string AccessionNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of specimen.
        /// </summary>
        public string SpecimenType { get; set; }
        /// <summary>
        /// Gets or sets the type of culture.
        /// </summary>
        public string CultureType { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient.
        /// </summary>
        public string PatientName { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer's barcode.
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// Gets or sets the status of the instrument result.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the result was received.
        /// </summary>
        public string ResultReceived { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the request was made.
        /// </summary>
        public string RequestMade { get; set; }

        /// <summary>
        /// Gets or sets additional data.
        /// </summary>
        public string MoreData { get; set; }

        /// <summary>
        /// Gets or sets the test results.
        /// </summary>
        public string TestResults { get; set; }

        /// <summary>
        /// Gets or sets the state id.
        /// </summary>
        public int StateId { get; set; }
    }

}
