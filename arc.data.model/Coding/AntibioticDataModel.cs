namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the antibiotic table in the database.
    /// </summary>
    public class AntibioticDataModel
    {
        /// <summary>
        /// Gets or sets the default code that will be used for the antibiotic.
        /// </summary>
        public string? Code {  get; set; }
        /// <summary>
        /// Gets or sets the antibiotic name.
        /// </summary>
        public string? AntibioticName { get; set; }

        /// <summary>
        /// Gets or sets foreign key linking the antibioticgroup table to the antibiotic table.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Gets or sets the ATC (Anatomical Therapeutic Chemical) code for the antibiotic.
        /// </summary>
        public string? Atc {  get; set; }

        /// <summary>
        /// Gets or sets the CID (Compound ID) for the antibiotic.
        /// </summary>
        public string? Cid { get; set; }

        /// <summary>
        /// Gets or sets the LOINC (Logical Observation Identifiers Names and Codes) code for the antibiotic.
        /// </summary>
        public string? Loinc { get; set; }
    }
}
