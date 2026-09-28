using arc.app.Asset;
using arc.app.Common;
using arc.common.Data;
using arc.common.Models;
using arc.data.Common;
using arc.data.model.Asset;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Asset;

/// <summary>
/// Repository class for handling storage-related operations.
/// </summary>
/// <remarks>
/// This class extends the <see cref="GeneralRepository"/> and implements <see cref="IStorageRepository"/> 
/// to provide functionality for adding, editing, and querying storage records.
/// </remarks>
public class StorageRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter, IGenerateMoreData moreDataGenerator)
    : GeneralRepository(sqlQuery, logWriter, sqlCommand), IStorageRepository
{
    /// <summary>
    /// Instance of the generator used to produce additional data for storage records.
    /// </summary>
    private readonly IGenerateMoreData _moreDataGenerator = moreDataGenerator;

    /// <summary>
    /// Adds a new storage record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The data to save, provided as a JSON string.</param>
    /// <param name="token">The user-specific token containing authentication and contextual information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added storage record.
    /// </returns>
    public async Task<int> AddAsync(string dataToSave, TokenInfoModel token)
    {
        var storage = JsonConvert.DeserializeObject<StorageDataModel>(dataToSave);
        storage.MoreData = _moreDataGenerator.GetMoreDataJsonString("storage", dataToSave);
        storage.LaboratoryId = string.IsNullOrEmpty(token.LaboratoryId) ? 0 : int.Parse(token.LaboratoryId);
        _logWriter.LogInfo("Run add storage command", "StorageRepository", "AddAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddStorageCommand(), "Add Storage", storage);
    }

    /// <summary>
    /// Edits an existing storage record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The updated storage data, provided as a JSON string.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the edited storage record.
    /// </returns>
    public async Task<int> EditAsync(string dataToSave)
    {
        var storage = JsonConvert.DeserializeObject<StorageDataModel>(dataToSave);
        storage.MoreData = _moreDataGenerator.GetMoreDataJsonString("storage", dataToSave);
        _logWriter.LogInfo("Run edit storage command", "StorageRepository", "EditAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new EditStorageCommand(), "Edit Storage", storage);
    }

    /// <summary>
    /// Retrieves a list of storage options asynchronously for dropdown lists.
    /// </summary>
    /// <param name="token">The user-specific token containing authentication and contextual information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an enumerable list of storage options.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetStorageForListAsync(TokenInfoModel token)
    {
        _logWriter.LogInfo("Run get storage location for dropdown list query", "StorageRepository", "GetLocationsForList");
        return await _sqlQuery.QueryReturningTypeAsync(new GetStorageForListQuery(), "Get storage for list Query", new QueryFilterConfig().AddString("laboratoryid", token.LaboratoryId));
    }
}

