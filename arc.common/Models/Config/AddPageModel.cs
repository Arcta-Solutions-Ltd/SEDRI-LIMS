namespace arc.common.Models.Config
{
    public class AddPageModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the database table that fields on this page are stored in
        /// (specimen, patient, admission, or request).
        /// </summary>
        public string TableName { get; set; }
    }
}
