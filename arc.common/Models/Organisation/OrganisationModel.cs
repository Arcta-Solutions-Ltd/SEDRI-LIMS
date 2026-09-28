namespace arc.common.Models.Organisation
{
    public class OrganisationModel
    {
        public string Id { get; set; }
        public string OrganisationName { get; set; }
        public string Code { get; set; }
        public string FullyQualifiedName { get; set; }
        public string OrganisationCodeHierarchy { get; set; }
        public string ParentOrganisationId { get; set; }
        public string LanguageId { get; set; }
        public string MoreData { get; set; }
        public string Enabled { get; set; }
        public int? LocationId { get; set; } = null;
    }
}
