using arc.app.Common;
using arc.app.Security;
using arc.common.Data;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Organisation;
using arc.common.Utils;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation;

/// <summary>
/// Repository responsible for handling organisation-related data operations.
/// </summary>
public class OrganisationRepository : IOrganisationRepository
{
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly IGenerateMoreData _moreDataGenerator;
    private readonly ILogWriter _logWriter;
    private readonly IJsonReplacer _jsonReplacer;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganisationRepository"/> class.
    /// </summary>
    /// <param name="sqlCommand">The SQL command executor.</param>
    /// <param name="sqlQuery">The SQL query executor.</param>
    /// <param name="moreDataGenerator">Generates additional JSON data for an organisation.</param>
    /// <param name="logWriter">Used for logging information and errors.</param>
    public OrganisationRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, IGenerateMoreData moreDataGenerator, ILogWriter logWriter, IJsonReplacer jsonReplacer)
    {
        _sqlCommand = sqlCommand;
        _sqlQuery = sqlQuery;
        _moreDataGenerator = moreDataGenerator;
        _logWriter = logWriter;
        _jsonReplacer = jsonReplacer;
    }

    /// <summary>
    /// Adds a new organisation using the provided JSON data.
    /// </summary>
    /// <param name="dataToSave">A JSON string containing the organisation data to be saved.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of affected rows.
    /// </returns>
    public async Task<int> AddAsync(string dataToSave)
    {
        var organisation = JsonConvert.DeserializeObject<OrganisationModel>(dataToSave);
        organisation.MoreData = _moreDataGenerator.GetMoreDataJsonString("organisation", dataToSave);
        _logWriter.LogInfo("Run add organisation command", "OrganisationRepository", "Add");
        var result = await _sqlCommand.CommandWithTypeQueryAsync(new AddOrganisationCommand(_jsonReplacer), "Add Organisation", organisation);
        _logWriter.LogInfo($"Add organisation completed; ParentOrganisationId={organisation.ParentOrganisationId ?? "null"}", "OrganisationRepository", "Add");
        return result;
    }

    /// <summary>
    /// Edits an existing organisation using the provided JSON data.
    /// </summary>
    /// <param name="dataToSave">A JSON string containing the updated organisation data.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the number of affected rows.
    /// </returns>
    public async Task<int> EditAsync(string dataToSave)
    {
        var organisation = JsonConvert.DeserializeObject<OrganisationModel>(dataToSave);
        organisation.MoreData = _moreDataGenerator.GetMoreDataJsonString("organisation", dataToSave);
        _logWriter.LogInfo("Run edit organisation command", "OrganisationRepository", "Edit");
        var result = await _sqlCommand.CommandWithTypeQueryAsync(new EditOrganisationCommand(_jsonReplacer), "Edit Organisation", organisation);
        if (!organisation.Enabled.IsOrganisationEnabled())
        {
            _logWriter.LogInfo($"Organisation disabled; Id={organisation.Id}, Name={organisation.OrganisationName}", "OrganisationRepository", "Edit");
        }
        return result;
    }

    /// <summary>
    /// Retrieves a filtered list of organisations for a dropdown based on token information.
    /// </summary>
    /// <param name="token">A token containing the laboratory and organisation IDs for filtering.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an enumerable of <see cref="OptionsConfig"/>.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetOrganisationsForListAsync(TokenInfoModel token)
    {
        var queryFilter = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig>
            {
                new QueryValuesConfig { Key = "laboratoryid", Value = token.LaboratoryId },
                new QueryValuesConfig { Key = "organisationid", Value = token.OrganisationId }
            }
        };
        _logWriter.LogInfo("Run organisation list for dropdown query", "OrganisationRepository", "GetOrganisationsForList");
        var result = await _sqlQuery.QueryReturningTypeAsync(new GetOrganisationsForListQuery(), "Get Organisations for list Query", queryFilter);
        _logWriter.LogInfo($"Organisation list for dropdown returned {result.Count()} option(s)", "OrganisationRepository", "GetOrganisationsForList");
        return result;
    }

    /// <summary>
    /// Retrieves an unfiltered list of organisations for a dropdown.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an enumerable of <see cref="OptionsConfig"/>.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetOrganisationsForListUnfilteredAsync()
    {
        _logWriter.LogInfo("Run unfiltered organisation list for dropdown query", "OrganisationRepository", "GetOrganisationsForListUnfiltered");
        return await _sqlQuery.QueryReturningTypeAsync(new GetOrganisationsForListUnfilteredQuery(), "Get Organisations for list unfiltered Query", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves the organisational hierarchy for a specified organisation.
    /// </summary>
    /// <param name="id">The organisation ID as a string.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a JSON string representing the organisation hierarchy.
    /// </returns>
    public async Task<string> GetOrganisationHierarchyAsync(string id)
    {
        var queryFilter = new QueryFilterConfig().AddString("organisationid", id);
        _logWriter.LogInfo("Run organisation hierarchy query", "OrganisationRepository", "GetOrganisationHierarchy");
        return await _sqlQuery.QueryReturningStringAsync(new OrganisationHierarchyListQuery(), "Get Organisation Hierarchy Query", queryFilter);
    }

    /// <summary>
    /// Retrieves the name and other details of an organisation for a given organisation ID.
    /// </summary>
    /// <param name="id">The organisation ID.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an <see cref="OptionsConfig"/> object.
    /// </returns>
    public async Task<OptionsConfig> GetOrganisationsForNameByIdAsync(int id)
    {
        var queryFilter = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = id.ToString() } }
        };
        _logWriter.LogInfo("Run organisation for name query", "OrganisationRepository", "GetOrganisationsForNameById");
        return await _sqlQuery.QueryReturningTypeAsync(new GetOrganisationForNameByIdQuery(), "Get organisation for name query", queryFilter);
    }

    /// <summary>
    /// Retrieves the organisation details required for validation based on the provided parameters.
    /// </summary>
    /// <param name="parameters">A <see cref="QueryFilterConfig"/> containing parameters for validation.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains an integer value,
    /// typically representing a count or status code.
    /// </returns>
    public async Task<int> GetOrganisationForValidationByIdAsync(QueryFilterConfig parameters)
    {
        _logWriter.LogInfo("Run organisation for validation query", "OrganisationRepository", "GetOrganisationForValidation");
        return await _sqlQuery.QueryReturningIntegerAsync(new GetOrganisationForValidationByIdQuery(), "Get organisation for validation query", parameters);
    }

    /// <summary>
    /// Retrieves the organisational code hierarchy for a specified organisation.
    /// </summary>
    /// <param name="parentOrganisationId">The organisation ID of the parent as a string.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains a JSON string representing the organisation hierarchy.
    /// </returns>
    public async Task<OrganisationModel> GetOrganisationForHierarchyAsync(string parentOrganisationId)
    {
        var queryFilter = new QueryFilterConfig().AddString("parentOrganisationId", parentOrganisationId);
        _logWriter.LogInfo("Run organisation hierarchy query", "OrganisationRepository", "GetOrganisationHierarchy");
        return await _sqlQuery.QueryReturningTypeAsync(new GetOrganisationForHierarchy(), "Get Organisation Hierarchy Query", queryFilter);
    }

    /// <summary>
    /// Determines whether the specified organisation exists and is enabled.
    /// </summary>
    /// <param name="organisationId">The organisation id to check.</param>
    /// <returns><c>true</c> when the organisation is enabled; otherwise <c>false</c>.</returns>
    public async Task<bool> IsOrganisationEnabledAsync(int organisationId)
    {
        var queryFilter = new QueryFilterConfig
        {
            Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = organisationId.ToString() } }
        };
        _logWriter.LogInfo($"Check organisation enabled state; Id={organisationId}", "OrganisationRepository", "IsOrganisationEnabled");
        var count = await _sqlQuery.QueryReturningIntegerAsync(new IsOrganisationEnabledByIdQuery(), "Is organisation enabled query", queryFilter);
        return count > 0;
    }
}