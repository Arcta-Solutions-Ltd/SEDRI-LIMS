using arc.common;
using arc.common.Models;
using arc.common.Models.Role;
using Newtonsoft.Json;
using System.Linq;

namespace arc.app.Roles
{
    public class EventPermissionsMapping : IMap
    {
        public string Map(string source)
        {
            var sourceData = JsonConvert.DeserializeObject<CraftedWithIdForEventModel<EventPermissionEventModel>>(source);
            
            var allowedList = sourceData.Crafted[0].Contents.Where(m => m.Allowed == "Yes").Select(m => m.Event).ToList();

            if (allowedList.Count() > 0)
            {
                var returnData = "'" + allowedList.First() + "'";
                for (int i = 1; i < allowedList.Count(); i++)
                {
                    returnData += ",'" + allowedList[i] + "'";
                }
                return @"{ 'Id': '" + sourceData.Id + "', 'EventPermission': { 'AllowedEvents' : [" + returnData + "]}}";
            } else {
                return @"{ 'Id': '" + sourceData.Id + "', 'EventPermission': { 'AllowedEvents' : []}}";

            }
        }
    }
}

