using arc.app.Common;
using arc.data.model.Asset;
using arc.domain.Location;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Asset;

/// <summary>
/// Represents a command to add a new storage record.
/// </summary>
internal class AddStorageCommand : ICommandWithTypeReturningInteger<StorageDataModel>
{
    /// <summary>
    /// Executes the command asynchronously to insert a new storage record into the database.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="storage">The storage data model containing the details to be added.</param>
    /// <param name="logWriter">The logger instance used for writing log information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added storage record.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, StorageDataModel storage, ILogWriter logWriter)
    {
        if (storage.ParentStorageId > 0)
        {
            var parentLocation = await connect.QueryFirstAsync<StorageDataModel>(
                @"select id, storagename, fullyqualifiedname, parentstorageid from storage where id = @ParentStorageId",
                new { storage.ParentStorageId }
            );
            storage.FullyQualifiedName = $"{parentLocation.FullyQualifiedName} : {storage.StorageName}";
        }
        else
        {
            storage.FullyQualifiedName = storage.StorageName;
        }

        var sql = @"
            insert into storage(
                storagename, fullyqualifiedname, parentstorageid, lastmodifieddate, moredata, 
                enabled, description, storagetypeid, code, temperature, LaboratoryId
            )
            values(
                @StorageName, @FullyQualifiedName, @ParentStorageId, now(), 
                cast(@MoreData as json), @Enabled, @Description, @StorageTypeId, @Code, @Temperature, @LaboratoryId
            )
            returning id";

        return await connect.ExecuteAsync(sql, storage);
    }
}


