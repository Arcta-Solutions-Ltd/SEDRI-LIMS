using arc.common.ExtensionMethods;
using System;
using System.Linq;

namespace arc.domain.Security.Role
{
    public class MenuPermissionDetails
    {
        public MenuPermissionDetails()
        {
            AllowedSidebarItems = Array.Empty<string>();
        }

        public string[] AllowedSidebarItems { get; set; }

        internal void CombineSidebarItems(string[] sidebarItems)
        {
            AllowedSidebarItems = AllowedSidebarItems.OrEmpty().Union(sidebarItems.OrEmpty()).ToArray();
        }
    }

    
}
