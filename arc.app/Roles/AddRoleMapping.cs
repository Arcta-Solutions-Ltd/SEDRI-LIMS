using arc.app.Security;
using arc.common;
using arc.common.Models;
using arc.common.Models.Role;
using arc.common.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Roles
{
    /// <summary>
    /// Maps Add Role workflow payloads into role records, including menu and event permission JSON.
    /// </summary>
    public class AddRoleMapping : IMap, IAddRoleMapping
    {
        private readonly ICraftedUtils _craftedUtils;
        private readonly IAddTopLevelItemsToMenuPermissions _addtopLevelMenuPermissions;
        private readonly IEnsureMandatoryMenuPermissions _ensureMandatoryMenuPermissions;

        public AddRoleMapping(ICraftedUtils craftedUtils, IAddTopLevelItemsToMenuPermissions addtopLevelMenuPermissions, IEnsureMandatoryMenuPermissions ensureMandatoryMenuPermissions)
        {
            _craftedUtils = craftedUtils;
            _addtopLevelMenuPermissions = addtopLevelMenuPermissions;
            _ensureMandatoryMenuPermissions = ensureMandatoryMenuPermissions;
        }

        /// <summary>
        /// Maps the Add Role event payload to a role insert/update shape, persisting explicit empty permission arrays when no toggles are enabled.
        /// </summary>
        /// <param name="source">The serialized Add Role event payload.</param>
        /// <returns>JSON describing the role to save.</returns>
        public string Map(string source)
        {
            var craftedInfo = _craftedUtils.ExtractCraftedFromJson(source);

            var sourceData = JsonConvert.DeserializeObject<AddRoleEventModel>(source);

            var returnValue = @"{ 'RoleName': '" + sourceData.RoleName + @"', 'RoleDescription': '" + sourceData.RoleDescription + @"',
                                    'Enabled': '" + sourceData.Enabled + @"'";

            var menuCrafted = craftedInfo?.FirstOrDefault(c => string.Equals(c.Name, "menupermission", StringComparison.OrdinalIgnoreCase));
            if (menuCrafted != null)
            {
                var sourceWithParents = _addtopLevelMenuPermissions.Add(source);
                var sourceWithMandatory = _ensureMandatoryMenuPermissions.EnsureInCraftedPayload(sourceWithParents);
                var sourceInfo = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<MenuPermissionEventModel>>(sourceWithMandatory);

                var allowedList = sourceInfo.Crafted[0].Contents.Where(m => m.Allowed == "Yes").Select(m => m.Key).ToList();

                if (allowedList.Count() > 0)
                {
                    var returnData = "'" + allowedList.First() + "'";
                    for (int i = 1; i < allowedList.Count(); i++)
                    {
                        returnData += ",'" + allowedList[i] + "'";
                    }
                    returnValue += @",'MenuPermission': { 'AllowedSidebarItems' : [" + returnData + @"]}";
                }
                else
                {
                    returnValue += @",'MenuPermission': { 'AllowedSidebarItems' : []}";
                }
            }

            var eventCrafted = craftedInfo?.FirstOrDefault(c => string.Equals(c.Name, "eventpermission", StringComparison.OrdinalIgnoreCase));
            if (eventCrafted != null)
            {
                var eventPermissions = JsonConvert.DeserializeObject<List<EventPermissionModel>>(eventCrafted.Contents);
                var allowedList = eventPermissions.Where(m => m.Allowed == "Yes").Select(m => m.Event).ToList();

                if (allowedList.Count() > 0)
                {
                    var returnData = "'" + allowedList.First() + "'";
                    for (int i = 1; i < allowedList.Count(); i++)
                    {
                        returnData += ",'" + allowedList[i] + "'";
                    }
                    returnValue += @",'EventPermission': { 'AllowedEvents' : [" + returnData + "]}";
                }
                else
                {
                    returnValue += @",'EventPermission': { 'AllowedEvents' : []}";
                }
            }

            return returnValue + "}";
        }
    }
}

