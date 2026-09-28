using arc.app.Common;
using arc.domain.Location;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Location;

/// <summary>
/// Represents a command to add a new location record.
/// </summary>
internal class AddLocationCommand : ICommandWithTypeReturningInteger<LocationModel>
{
    /// <summary>
    /// Executes the command asynchronously to insert a new location record into the database.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="location">The location data model containing the details to be added.</param>
    /// <param name="logWriter">The logger instance used for writing log information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added location record.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, LocationModel location, ILogWriter logWriter)
    {
        if (location.ParentLocationId > 0)
        {
            var parentLocation = await connect.QueryFirstAsync<LocationModel>(
                @"select id, name, fullyqualifiedname, parentlocationid, moredata ->> 'locationcodehierarchy' as locationcodehierarchy from location where id = @ParentLocationId",
                new { location.ParentLocationId }
            );
            location.FullyQualifiedName = $"{parentLocation.FullyQualifiedName} : {location.Name}";
            location.MoreData = $"{{\r\n  \"locationcodehierarchy\": \"{parentLocation.LocationCodeHierarchy} : {location.Code}\"\r\n}}";
        }
        else
        {
            location.FullyQualifiedName = location.Name;
            location.MoreData = $"{{\r\n  \"locationcodehierarchy\": \"{location.Code}\"\r\n}}";
        }

        var sql = @"
            insert into location(
                name, fullyqualifiedname, parentlocationid, lastmodifieddate, 
                moredata, enabled, longitude, latitude, code
            )
            values(
                @Name, @FullyQualifiedName, @ParentLocationId, now(), 
                cast(@MoreData as json), @Enabled, @Longitude, @Latitude, @Code
            )
            returning id";

        return await connect.ExecuteAsync(sql, location);
    }
}
