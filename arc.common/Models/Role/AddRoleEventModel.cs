namespace arc.common.Models.Role
{
    public class AddRoleEventModel 
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string SystemAdmin { get; set; }
        public string OrganisationAdmin { get; set; }
        public string LaboratoryAdmin { get; set; }
        public string Configuration { get; set; }
        public string Enabled { get; set; }
        public string MoreData { get; set; }
    }
}
