using System;

namespace arc.data.Security
{
    public class OrganisationAdminUserDataModel
    {
        public string OrgAdminUsername { get; set; }
        public string OrgAdminFirstName { get; set; }
        public string OrgAdminLastName { get; set; }
        public string Enabled { get; set; }
        public string OrgAdminPassword { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
