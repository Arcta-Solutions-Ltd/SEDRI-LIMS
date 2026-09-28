using arc.common.Models;
using arc.domain.Configuration.ListsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Asset;
/// <summary>
/// Defines the contract for a storage repository to handle storage-related operations.
/// </summary>
public interface IStorageRepository
{
    /// <summary>
    /// Adds a new storage record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The data to save, provided as a JSON string.</param>
    /// <param name="token">The user-specific token containing authentication and contextual information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the newly added storage record.
    /// </returns>
    Task<int> AddAsync(string dataToSave, TokenInfoModel token);

    /// <summary>
    /// Edits an existing storage record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The updated storage data, provided as a JSON string.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the ID of the edited storage record.
    /// </returns>
    Task<int> EditAsync(string dataToSave);

    /// <summary>
    /// Retrieves a list of storage options asynchronously.
    /// </summary>
    /// <param name="token">The user-specific token containing authentication and contextual information.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an enumerable list of storage options.
    /// </returns>
    Task<IEnumerable<OptionsConfig>> GetStorageForListAsync(TokenInfoModel token);
}

