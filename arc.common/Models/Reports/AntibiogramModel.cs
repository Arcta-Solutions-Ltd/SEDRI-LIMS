namespace arc.common.Models.Reports
{
    /// <summary>
    /// Represents the model for an antibiogram.
    /// </summary>
    public class AntibiogramModel
    {
        /// <summary>
        /// Gets or sets the name of the antibiotic.
        /// </summary>
        public string AntibioticName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the antibiotic.
        /// </summary>
        public int AntibioticId { get; set; }

        /// <summary>
        /// Gets or sets the name of the organism.
        /// </summary>
        public string OrganismName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the organism.
        /// </summary>
        public int OrganismId { get; set; }

        /// <summary>
        /// Gets or sets the total number of samples.
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// Gets or sets the number of resistant samples.
        /// </summary>
        public int Resistant { get; set; }

        /// <summary>
        /// Gets or sets the percentage of resistant samples.
        /// </summary>
        public int Percentage { get; set; }
    }
}
