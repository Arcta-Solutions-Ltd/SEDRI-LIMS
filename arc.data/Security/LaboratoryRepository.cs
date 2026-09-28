using arc.app.Common;
using arc.app.Security;
using arc.common.Models;
using arc.common.Models.Laboratory;
using arc.data.Common;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Repository class for managing laboratory-related operations.
/// </summary>
public class LaboratoryRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
    : GeneralRepository(sqlQuery, logWriter, sqlCommand), ILaboratoryRepository
{
    /// <summary>
    /// Retrieves a list of laboratories for a dropdown based on token information.
    /// </summary>
    /// <param name="token">The token containing laboratory-related information.</param>
    /// <returns>An enumerable list of <see cref="OptionsConfig"/> representing laboratories for the dropdown.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetLaboratoriesForListAsync(TokenInfoModel token)
    {
        var queryFilter = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig> {
                new QueryValuesConfig { Key = "laboratoryid", Value = token.LaboratoryId }
            }
        };
        _logWriter.LogInfo("Run laboratory list for dropdown query", "LaboratoryRepository", "GetLaboratoriesForList");
        return await _sqlQuery.QueryReturningTypeAsync(new GetLaboratoriesForListQuery(), "Get Laboratory List for dropdown", queryFilter);
    }

    /// <summary>
    /// Retrieves an unfiltered list of laboratories for a dropdown.
    /// </summary>
    /// <returns>An enumerable list of <see cref="OptionsConfig"/> representing laboratories for the dropdown.</returns>
    public async Task<IEnumerable<OptionsConfig>> GetLaboratoriesForListUnfilteredAsync()
    {
        _logWriter.LogInfo("Run unfiltered laboratory list for dropdown query", "LaboratoryRepository", "GetLaboratoriesForListUnfiltered");
        return await _sqlQuery.QueryReturningTypeAsync(new GetLaboratoriesForListUnfilteredQuery(), "Get Laboratory List for dropdown", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves a complete list of laboratories based on query filters.
    /// </summary>
    /// <param name="queryFilters">The filters to apply to the laboratory list query.</param>
    /// <returns>An enumerable list of <see cref="LaboratoryListModel"/> representing laboratories.</returns>
    public async Task<IEnumerable<LaboratoryListModel>> GetLaboratoryListAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run laboratory list query", "LaboratoryRepository", "GetLaboratoryListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new LaboratoryListQuery(), "Get Laboratory List", queryFilters);
    }

    /// <summary>
    /// Retrieves details of a single laboratory by its identifier.
    /// </summary>
    /// <param name="queryFilters">The filters containing the laboratory identifier.</param>
    /// <returns>A <see cref="LaboratoryListModel"/> representing the laboratory details.</returns>
    public async Task<LaboratoryListModel> LaboratoryByIdAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run laboratory by id query", "LaboratoryRepository", "LaboratoryByIdAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new LaboratoryByIdQuery(), "Get Single Laboratory", queryFilters);
    }

    /// <summary>
    /// Deletes a laboratory record based on its identifier.
    /// </summary>
    /// <param name="id">The identifier of the laboratory to delete.</param>
    /// <returns>An asynchronous task for the deletion operation.</returns>
    public async Task DeleteLaboratoryAsync(string id)
    {
        _logWriter.LogInfo("Run delete laboratory command", "LaboratoryRepository", "DeleteLaboratoryAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteLaboratoryCommand(), "Delete laboratory", id);
    }
}