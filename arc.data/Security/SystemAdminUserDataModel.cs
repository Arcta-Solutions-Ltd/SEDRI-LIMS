using System;

namespace arc.data.Security
{
    public class SystemAdminUserDataModel
    {
        public string SystemAdminUsername { get; set; }
        public string SystemAdminFirstName { get; set; }
        public string SystemAdminLastName { get; set; }
        public string Enabled { get; set; }
        public string SystemAdminPassword { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
