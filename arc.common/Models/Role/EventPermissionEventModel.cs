using System.Collections.Generic;

namespace arc.common.Models.Role
{
    public class EventPermissionEventModel
    {
        public int Id { get; set; }
        public List<EventPermissionModel> Contents { get; set; }
    }
}
