using System.Collections.Generic;

namespace arc.domain.Security.Permission
{
    public class ViewPermission
    {
        public bool Enabled { get; set; }
        public IEnumerable<ButtonPermission> ButtonPermissions { get; set; }
    }
}
