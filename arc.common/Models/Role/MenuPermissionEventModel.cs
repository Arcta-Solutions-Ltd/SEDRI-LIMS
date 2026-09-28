using System.Collections.Generic;

namespace arc.common.Models.Role
{
    public class MenuPermissionEventModel
    {
        public string Name { get; set; }
        public List<CraftedSelectionsModel> Contents { get; set; }

    }
}
