namespace arc.common.Models.Alert
{
    public class OrganismAlertListModel
    {
        public int Id { get; set; }
        public string AlertName { get; set; }
        public string AlertMessage { get; set; }
        public string Enabled { get; set; }
        public int AlertTypeId { get; set; }
        /// <summary>
        /// Gets or sets the specification ID (FK to specification).
        /// </summary>
        public int SpecificationId { get; set; }
    }
}
