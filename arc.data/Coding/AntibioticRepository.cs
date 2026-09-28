using arc.app.Coding;
using arc.app.Common;
using arc.common.Models.Coding;
using arc.domain.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Repository class for managing antibiotic-related operations such as adding, retrieving, 
/// and deleting antibiotic entries.
/// </summary>
public class AntibioticRepository : IAntibioticRepository
{
    private readonly ISqlQuery _sqlQuery;
    private readonly ISqlCommand _sqlCommand;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="AntibioticRepository"/> class.
    /// </summary>
    /// <param name="sqlQuery">The SQL query executor.</param>
    /// <param name="logWriter">The log writer for logging repository operations.</param>
    /// <param name="sqlCommand">The SQL command executor.</param>
    public AntibioticRepository(ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand)
    {
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
        _sqlCommand = sqlCommand;
    }

    /// <summary>
    /// Adds a new antibiotic entry to the repository.
    /// </summary>
    /// <param name="dataToSave">A JSON string containing the antibiotic data.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the identifier 
    /// of the newly added antibiotic.
    /// </returns>
    public async Task<int> AddAntibioticAsync(string dataToSave)
    {
        var data = JsonConvert.DeserializeObject<Antibiotic>(dataToSave);
        _logWriter.LogInfo("Run add antibiotic command", "AntibioticRepository", "AddAntibioticAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddAntibioticCommand(), "Insert Antibiotic", data, _logWriter);
    }

    /// <summary>
    /// Retrieves a list of antibiotics formatted as options for dropdowns.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection 
    /// of <see cref="OptionsConfig"/> objects representing antibiotics.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetAntibioticListAsync()
    {
        _logWriter.LogInfo("Run get antibiotic list query", "AntibioticRepository", "GetAntibioticListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new AntibioticListQuery(), "Get antibiotic list", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves a list of antibiotics for view purposes.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of 
    /// <see cref="AntibioticListModel"/> objects.
    /// </returns>
    public async Task<List<AntibioticListModel>> GetAntibioticListForViewAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get antibiotic list for view query", "AntibioticRepository", "GetAntibioticListForViewAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new AntibioticListForViewQuery(), "Get antibiotic list for view", queryFilters);
    }

    /// <summary>
    /// Retrieves a single antibiotic entry based on the coding identifier.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration containing the coding identifier.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Antibiotic"/> entry.
    /// </returns>
    public async Task<Antibiotic> GetAntibioticEntryByCodingIdAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get antibiotic entry by coding id query", "AntibioticRepository", "GetAntibioticEntryByCodingIdAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new AntibioticEntryByCodingIdQuery(), "Get antibiotic entry by coding id", queryFilters);
    }

    /// <summary>
    /// Retrieves a single antibiotic entry using its identifier.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration containing the antibiotic identifier.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an 
    /// <see cref="AntibioticListModel"/> representing the antibiotic entry.
    /// </returns>
    public async Task<AntibioticListModel> GetAntibioticEntryByIdAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get antibiotic entry by id query", "AntibioticRepository", "GetAntibioticEntryByIdAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new AntibioticEntryByIdQuery(), "Get antibiotic entry by id", queryFilters);
    }

    /// <summary>
    /// Retrieves a single antibiotic entry by its code.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration containing the antibiotic code.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an 
    /// <see cref="AntibioticListModel"/> representing the antibiotic entry.
    /// </returns>
    public async Task<AntibioticListModel> GetAntibioticEntryByCodeAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get antibiotic entry by code query", "AntibioticRepository", "GetAntibioticEntryByCodeAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new AntibioticEntryByCodeQuery(), "Get antibiotic entry by code", queryFilters);
    }

    /// <summary>
    /// Retrieves a list of antibiotics along with their associated groups.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection 
    /// of <see cref="OptionsConfig"/> objects including antibiotic and group information.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetAntibioticListWithGroupsAsync()
    {
        _logWriter.LogInfo("Run get antibiotic list with groups query", "AntibioticRepository", "GetAntibioticListWithGroupsAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new AntibioticListWithGroupsQuery(), "Get antibiotic list with groups", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves a list of resistant antibiotics based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The query filter configuration for retrieving resistant antibiotics.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection 
    /// of <see cref="OptionsConfig"/> objects representing resistant antibiotics.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetResistantAntibioticQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get resistant antibiotic query", "AntibioticRepository", "GetResistantAntibioticQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ResistantAntibioticQuery(), "Get resistant antibiotic list", queryFilters);
    }

    /// <summary>
    /// Deletes an antibiotic group identified by the specified identifier.
    /// </summary>
    /// <param name="id">The identifier of the antibiotic group to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task DeleteAntibioticGroupAsync(string id)
    {
        _logWriter.LogInfo("Run delete antibiotic group command", "AntibioticRepository", "DeleteAntibioticGroupAsync");
        await _sqlCommand.CarryOutCommandAsync(new DeleteAntibioticGroupCommand(), "Delete antibiotic group", id);
    }

    /// <summary>
    /// Retrieves antibiotic display names for the supplied ids.
    /// </summary>
    /// <param name="ids">Antibiotic ids to resolve.</param>
    /// <returns>A dictionary mapping id to <c>antibioticname</c>.</returns>
    public async Task<Dictionary<int, string>> GetAntibioticNamesByIdsAsync(IEnumerable<int> ids)
    {
        var idList = ids?.Where(x => x > 0).Distinct().ToList() ?? [];
        if (idList.Count == 0)
        {
            return new Dictionary<int, string>();
        }

        _logWriter.LogInfo("Run get antibiotic names by ids query", "AntibioticRepository", "GetAntibioticNamesByIdsAsync");
        var queryFilters = new QueryFilterConfig().AddString("ids", string.Join(",", idList));
        var rows = await _sqlQuery.QueryReturningTypeAsync(
            new AntibioticNamesByIdsQuery(),
            "Get antibiotic names by ids",
            queryFilters);

        return rows
            .Where(row => row.Id > 0 && !string.IsNullOrEmpty(row.AntibioticName))
            .ToDictionary(row => row.Id, row => row.AntibioticName);
    }
}