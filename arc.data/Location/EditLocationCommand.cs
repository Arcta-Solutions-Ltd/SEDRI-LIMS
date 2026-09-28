using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.data.Utils;
using arc.domain.Location;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Location
{
    /// <summary>
    /// Updates a location's details and recomputes fully-qualified names and
    /// code hierarchies for the location and all of its descendants.
    /// </summary>
    internal class EditLocationCommand : ICommandWithTypeReturningInteger<LocationModel>
    {
        /// <summary>
        /// Executes the edit operation for a location.
        /// </summary>
        /// <param name="connect">Open database connection.</param>
        /// <param name="location">Location payload with edits.</param>
        /// <param name="logWriter">Logger for diagnostics.</param>
        /// <returns>The edited location's Id.</returns>
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, LocationModel location, ILogWriter logWriter)
        {
            if (location.ParentLocationId > 0)
            {
                var parentSql = @"select id, name, fullyqualifiedname, parentlocationid, moredata ->> 'locationcodehierarchy' as locationcodehierarchy from location where id = @Id";
                var parentOrg = await connect.QueryFirstAsync<LocationModel>(parentSql, new {Id = location.ParentLocationId});
                location.FullyQualifiedName = parentOrg.FullyQualifiedName + " : " + location.Name;
                location.MoreData = $"{{\r\n  \"locationcodehierarchy\": \"{parentOrg.LocationCodeHierarchy} : {location.Code}\"\r\n}}";
                location.LocationCodeHierarchy = $"{parentOrg.LocationCodeHierarchy} : {location.Code}";
            }
            else
            {
                location.FullyQualifiedName = location.Name;
                location.LocationCodeHierarchy = location.Code;
                location.MoreData = $"{{\r\n  \"locationcodehierarchy\": \"{location.Code}\"\r\n}}";

            }

            var sql = @"update location set name = @Name, fullyqualifiedname = @FullyQualifiedName, enabled = @Enabled, longitude = @Longitude, latitude = @Latitude,
                        parentlocationid = @ParentLocationId, lastmodifieddate = now(), moredata = cast(@MoreData As json), code = @Code Where  Id = @Id";

            await connect.ExecuteAsync(sql, location);

            // Find and update the fully-qualified name of all dependent organisations.
            var locationQuery = new LocationIdListFromLocationIdQuery();
            var locationIds = await locationQuery.Execute(connect, location.Id);
            var locationString = SqlSanitizer.SanitizeStringList(string.Join(",", locationIds));
            sql = @"select id, name, fullyqualifiedname, parentlocationid, moredata ->> 'locationcodehierarchy' as locationcodehierarchy, code from location where Id in (" + locationString + ") ";
            var result = await connect.QueryAsync<LocationModel>(sql);
            var locationList = result.ToList();

            foreach (var loc in locationList)
            {
                if (loc.Id == location.Id) { continue; }
                var locationWalker = loc;
                var fullyQualifiedName = locationWalker.Name;
                var locationCodeHierarchy = locationWalker.Code;
                while (locationWalker.ParentLocationId != location.Id)
                {
                    locationWalker = locationList.Where(o => o.Id == locationWalker.ParentLocationId).First();
                    fullyQualifiedName = locationWalker.Name + " : " + fullyQualifiedName;
                    locationCodeHierarchy = locationWalker.Code + " : " + locationCodeHierarchy;
                }
                fullyQualifiedName = location.FullyQualifiedName + " : " + fullyQualifiedName;
                locationCodeHierarchy = location.LocationCodeHierarchy + " : " + locationCodeHierarchy;
                var moreData = $"{{\r\n  \"locationcodehierarchy\": \"{locationCodeHierarchy}\"\r\n}}";

                sql = @"update location set fullyqualifiedname = @FullyQualifiedName, lastmodifieddate = now(), moredata = cast(@MoreData As json) Where  Id = @Id";

                await connect.ExecuteAsync(sql, new
                {
                    loc.Id,
                    fullyQualifiedName,
                    moreData
                });
            }

            return location.Id;
        }
    }
}
