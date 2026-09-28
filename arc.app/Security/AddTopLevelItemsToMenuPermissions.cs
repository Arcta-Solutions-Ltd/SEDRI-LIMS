using arc.app.Config.Sidebar;
using arc.common.Models;
using arc.common.Models.Role;
using Newtonsoft.Json;
using System.Linq;

namespace arc.app.Security
{
    /// <summary>
    /// Adds parent sidebar keys to menu permission payloads when any child menu is enabled.
    /// </summary>
    public class AddTopLevelItemsToMenuPermissions : IAddTopLevelItemsToMenuPermissions
    {
        /// <summary>
        /// Adds parent sidebar keys for any enabled child menu items in the crafted payload.
        /// </summary>
        /// <param name="source">The serialized Menu Permissions event payload.</param>
        /// <returns>The payload with parent menu keys added where required.</returns>
        public string Add(string source)
        {
            var returnMessage = "";

            var sidebarConfig = new SidebarSetup().GetLeftSidebarConfig();

            var sourceData = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<MenuPermissionEventModel>>(source);

            //var list = sourceData.Crafted[0].Contents.Where(m => m.Allowed == "Yes").Select(m => m.Key).ToList();

            var itemsWithChildren = sidebarConfig.Links.Where(l => l.Links != null);

            foreach(var item in itemsWithChildren)
            {
                var includeItem = false;
                foreach( var childItem in item.Links)
                {
                    var foundRecord = sourceData.Crafted[0].Contents.Where(c => c.Key.ToLower() == childItem.Key && c.Allowed == "Yes");
                    if (foundRecord.Count() > 0)
                    {
                        includeItem = true;
                    };
                }

                if (includeItem)
                {
                    var newItem = new CraftedSelectionsModel { Key = item.Key, Name = item.Name, Allowed = "Yes" };
                    sourceData.Crafted[0].Contents.Add(newItem);
                }
            }

            returnMessage = JsonConvert.SerializeObject(sourceData);

            return returnMessage;
        } 

    }
}
