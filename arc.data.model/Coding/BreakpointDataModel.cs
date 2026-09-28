namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the breakpoint table in the database.
    /// </summary>
    public class BreakpointDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets foreign key linking the order table to the breakpoint table.
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the family table to the breakpoint table.
        /// </summary>
        public int FamilyId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the organism table to the breakpoint table.
        /// </summary>
        public int OrganismId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the breakpoint table. This links to the organism group list in the listitem table.
        /// </summary>
        public int OrgGroupCodingId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the antibiotic table to the breakpoint table.
        /// </summary>
        public int AntibioticId { get; set; }
        /// <summary>
        /// Gets or sets the value of the dosage for the breakpoint.
        /// </summary>
        public string? Dosage { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the breakpoint table. This links to the test method list in the listitem table.
        /// </summary>
        public int TestMethodId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the specification table to the breakpoint table.
        /// The specification's guidelinesid corresponds to the source/guidelines (e.g. CLSI or EUCAST).
        /// </summary>
        public int SpecificationId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the breakpoint table. This links to the special consideration list in the listitem table.
        /// </summary>
        public int SpecialConsiderId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the breakpoint table. This links to the host list (typically Human or Animal), in the listitem table.
        /// </summary>
        public int HostId { get; set; }
        /// <summary>
        /// Gets or sets the value of the Enabled flag ('Yes' or 'No'), for the breakpoint. An enabled breakpoint will be used on the AST screen when calculating susceptibilities.
        /// </summary>
        public string? Enabled { get; set; }
        /// <summary>
        /// Gets or sets the value of the MakeDefault flag ('Yes' or 'No'), for the breakpoint. When more than one breakpoint matches the criteria for selection when calculating susceptibilities the one where MakeDefault is 'Yes' will be used.
        /// </summary>
        public string? MakeDefault { get; set; }
    }
}
