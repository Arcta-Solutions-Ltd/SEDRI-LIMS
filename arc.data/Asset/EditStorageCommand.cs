using arc.app.Common;
using arc.data.model.Asset;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Asset;

/// <summary>
/// Command for updating a storage record in the database and returning its ID.
/// </summary>
internal class EditStorageCommand : ICommandWithTypeReturningInteger<StorageDataModel>
{
    /// <summary>
    /// Executes the command asynchronously to update a storage record with the given data.
    /// </summary>
    /// <param name="connect">The database connection to use for the update operation.</param>
    /// <param name="storage">The <see cref="StorageDataModel"/> containing the updated storage data.</param>
    /// <param name="logWriter">The log writer for logging operations (not used in this implementation).</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the updated storage record.
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
            update storage set storagename = @storagename, fullyqualifiedname = @FullyQualifiedName, parentstorageid = @ParentStorageId,
                lastmodifieddate = now(), moredata = cast(@MoreData as json), Enabled = @Enabled, Description = @Description,
                storagetypeid = @StorageTypeId, code = @Code, temperature = @Temperature
            where id = @Id returning id";

        return await connect.ExecuteAsync(sql, storage);
    }
}

