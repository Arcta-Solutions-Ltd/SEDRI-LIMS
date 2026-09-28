namespace arc.data.model.Coding
{
    /// <summary>
    /// Represents the fields in the alert table in the database.
    /// </summary>
    public class AlertDataModel : IdAndDateBase
    {
        /// <summary>
        /// Gets or sets the name of the alert.
        /// </summary>
        public required string AlertName { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the order table to the alert table.
        /// </summary>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the family table to the alert table.
        /// </summary>
        public int FamilyId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the organism table to the alert table.
        /// </summary>
        public int OrganismId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alert table. This links to the organism group list in the listitem table.
        /// </summary>
        public int OrgGroupCodingId { get; set; }
        /// <summary>
        /// Gets or sets Yes/No value specifying whether the alert should be triggered when the organism exists.
        /// </summary>
        public string? DoesExist { get; set; }
        /// <summary>
        /// Gets or sets the message that will be displayed when the alert is triggered.
        /// </summary>
        public string? AlertMessage { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alert table to define how test conditions should be combined to trigger an alert. This links to the andor list in the listitem table.
        /// </summary>
        public string? TestAndOr { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alert table to define how susceptibilility conditions should be combined to trigger an alert. This links to the andor list in the listitem table.
        /// </summary>
        public string? SusceptibilityAndOr { get; set; }
        /// <summary>
        /// Gets or sets the value of the Enabled flag ('Yes' or 'No'), for the alert. Only an alert where Enabled is set to 'Yes' will be triggered.
        /// </summary>
        public string? Enabled { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the alerttype table to the alert table.
        /// </summary>
        public int AlertTypeId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the listitem table to the alert table. This links to the tag list in the listitem table.
        /// </summary>
        public string? TagId { get; set; }
        /// <summary>
        /// Gets or sets foreign key linking the specification table to the alert table. The specification's guidelinesid references the guidelines list (typically CLSI or EUCAST).
        /// </summary>
        public int SpecificationId { get; set; }
        /// <summary>
        /// Gets or sets the json that should be stored in the MoreData field.
        /// </summary>
        public string? MoreData { get; set; }

    }
}

