using System.Collections.Generic;

namespace arc.common.Models.Role
{
    public class AddRoleQueryModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Enabled { get; set; }
        public List<CraftedModel> Crafted {get; set;}
        public string MoreData { get; set; }
    }
}
