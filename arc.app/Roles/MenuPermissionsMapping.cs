using arc.app.Security;
using arc.common;
using arc.common.Models;
using arc.common.Models.Role;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace arc.app.Roles
{
    /// <summary>
    /// Maps menu permission crafted toggles to role menu permission JSON.
    /// </summary>
    public class MenuPermissionsMapping : IMap
    {
        private readonly IAddTopLevelItemsToMenuPermissions _addtopLevelMenuPermissions;
        private readonly IEnsureMandatoryMenuPermissions _ensureMandatoryMenuPermissions;

        public MenuPermissionsMapping(IAddTopLevelItemsToMenuPermissions addtopLevelMenuPermissions, IEnsureMandatoryMenuPermissions ensureMandatoryMenuPermissions)
        {
            _addtopLevelMenuPermissions = addtopLevelMenuPermissions;
            _ensureMandatoryMenuPermissions = ensureMandatoryMenuPermissions;
        }

        /// <summary>
        /// Maps menu permission toggles to a role update payload, including an explicit empty array when all toggles are off.
        /// </summary>
        /// <param name="source">The serialized Menu Permissions event payload.</param>
        /// <returns>JSON describing the role menu permission update.</returns>
        public string Map(string source)
        {
            try
            {
                var sourceWithParents = _addtopLevelMenuPermissions.Add(source);
                var sourceWithMandatory = _ensureMandatoryMenuPermissions.EnsureInCraftedPayload(sourceWithParents);

                var sourceData = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<MenuPermissionEventModel>>(sourceWithMandatory);
 
                var allowedList = sourceData.Crafted[0].Contents.Where(m => m.Allowed == "Yes").Select(m => m.Key).ToList();

                if (allowedList.Count() > 0)
                {
                    var returnData = "'" + allowedList.First() + "'";
                    for (int i = 1; i < allowedList.Count(); i++)
                    {
                        returnData += ",'" + allowedList[i] + "'";
                    }
                    return @"{ 'Id': '" + sourceData.Id + "', 'MenuPermission': { 'AllowedSidebarItems' : [" + returnData + "]}}";
                } else
                {
                    return @"{ 'Id': '" + sourceData.Id + "', 'MenuPermission': { 'AllowedSidebarItems' : []}}";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
