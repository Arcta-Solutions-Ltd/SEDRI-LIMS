using arc.app.Common;
using arc.app.SystemConfig;
using arc.common.Models.Config;
using arc.common.Models.SystemConfig;
using arc.data.model.Configuration;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.SystemConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// Repository class for handling configuration-related data operations by executing SQL queries and commands.
/// </summary>
public class ConfigRepository : IConfigRepository
{
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigRepository"/> class.
    /// </summary>
    /// <param name="sqlCommand">The SQL command executor.</param>
    /// <param name="sqlQuery">The SQL query executor.</param>
    /// <param name="logWriter">The log writer passed to commands that report what they wrote.</param>
    public ConfigRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
    {
        _sqlCommand = sqlCommand;
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Retrieves a list of all configuration data models.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="ConfigsDataModel"/>.
    /// </returns>
    public async Task<IEnumerable<ConfigsDataModel>> AllConfigListAsync()
    {
        return await _sqlQuery.QueryReturningTypeAsync(new ConfigListQuery(), "Get Config List", new QueryFilterConfig());
    }

    /// <summary>
    /// Retrieves a list of configuration models filtered by the specified query parameters.
    /// </summary>
    /// <param name="queryFilters">The filters to apply to the configuration query.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="ConfigsModel"/>.
    /// </returns>
    public async Task<IEnumerable<ConfigsModel>> GetConfigListAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new ConfigListByTypeForListsQuery(), "Get Config List by type", queryFilters);
    }

    /// <summary>
    /// Retrieves a single configuration model using the configuration name specified in the query filters.
    /// </summary>
    /// <param name="queryFilters">The filters containing the target configuration name.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ConfigsModel"/> instance.
    /// </returns>
    public async Task<ConfigsModel> SingleConfigByNameAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new SingleConfigByNameQuery(), "Get Single Config Item using name", queryFilters);
    }

    /// <summary>
    /// Retrieves a single configuration data model using the configuration ID specified in the query filters.
    /// </summary>
    /// <param name="queryFilters">The filters containing the target configuration ID.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="ConfigsDataModel"/> instance.
    /// </returns>
    public async Task<ConfigsDataModel> SingleConfigByIdAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new SingleConfigByIdQuery(), "Get Single Config Item using Id", queryFilters);
    }

    /// <summary>
    /// Adds a new custom configuration entry.
    /// </summary>
    /// <param name="dataToSave">
    /// A JSON string representing the configuration data to save.
    /// The data is deserialized into a <see cref="ConfigsModel"/> prior to insertion.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the identifier of the newly inserted configuration.
    /// </returns>
    public async Task<int> AddCustomEntryAsync(string dataToSave)
    {
        var data = JsonConvert.DeserializeObject<ConfigsModel>(dataToSave);
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddConfigCommand(), "Insert Config Item", data);
    }

    /// <summary>
    /// Edits an existing custom configuration entry.
    /// </summary>
    /// <param name="data">An instance of <see cref="ConfigsDataModel"/> containing the updated configuration data.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task EditCustomEntryAsync(ConfigsDataModel data)
    {
        await _sqlCommand.CommandWithTypeQueryAsync(new EditConfigCommand(), "Edit Config Item", data);
    }

    /// <summary>
    /// Deletes a custom configuration entry identified by its name.
    /// </summary>
    /// <param name="name">The name of the configuration entry to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    public async Task DeleteCustomEntryAsync(string name)
    {
        await _sqlCommand.CarryOutCommandAsync(new DeleteConfigCommand(), "Delete Config Item", name);
    }

    /// <summary>
    /// Updates an existing custom configuration entry.
    /// </summary>
    /// <param name="data">An instance of <see cref="ConfigsDataModel"/> containing the updated data.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    public async Task UpdateCustomEntryAsync(ConfigsDataModel data)
    {
        await _sqlCommand.CommandWithTypeQueryAsync(new UpdateConfigCommand(), "Update Config Item", data);
    }

    /// <summary>
    /// Retrieves the next available configuration name that is not already in use.
    /// </summary>
    /// <param name="queryFilters">The filters to apply when determining the next available name.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is the next available configuration name as a string.
    /// </returns>
    public async Task<string> GetNextAvailableNameAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningStringAsync(new NextFreeNameQuery(), "Get next available config name", queryFilters);
    }


        public async Task<string> GetNextAccessionNumberAsync(string prefix, string seedMask, int numberLength, string category)

    {
        var queryFilter = new QueryFilterConfig();
        queryFilter.AddString("prefix", prefix);
        queryFilter.AddString("seedMask", seedMask);
        queryFilter.AddInteger("numberLength", numberLength);
            queryFilter.AddString("category", category);

        return await _sqlQuery.QueryReturningStringAsync(new GetNextAccessionNumberQuery(), "Get next accession number query", queryFilter);
    }

    /// <summary>
    /// Retrieves a complete list of names.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="NameList"/>.
    /// </returns>
    public async Task<IEnumerable<NameList>> AllNameListAsync()
    {
        return await _sqlQuery.QueryReturningTypeAsync(new AllNameListQuery(), "Get all name list", new QueryFilterConfig());
    }

    /// <summary>
    /// Imports configuration data from a JSON string.
    /// </summary>
    /// <param name="dataToSave">A JSON string containing the configuration data to import.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result typically contains an identifier or status code for the import.
    /// </returns>
    public async Task<int> ImportConfigurationAsync(string dataToSave)
    {
        var data = JsonConvert.DeserializeObject<ImportConfigurationModel>(dataToSave);
        return await _sqlCommand.CommandWithTypeQueryAsync(new ImportConfigurationCommand(), "Import configuration", data.Upload);
    }

    /// <summary>
    /// Retrieves a list of mapping list view models based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The filters to apply when querying for the mapping list.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="MappingListViewModel"/>.
    /// </returns>
    public async Task<List<MappingListViewModel>> GetMappingListAsync(QueryFilterConfig queryFilters)
    {
        return await _sqlQuery.QueryReturningTypeAsync(new GetMappingListViewQuery(), "Get Config List for mapping list view", queryFilters);
    }

    /// <summary>
    /// Retrieves a list of workflow models.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="WorkflowListModel"/>.
    /// </returns>
    public async Task<IEnumerable<WorkflowListModel>> GetWorkflowListAsync()
    {
        return await _sqlQuery.QueryReturningTypeAsync(new WorkflowListQuery(), "Get Workflow List", new QueryFilterConfig());
    }

    /// <summary>
    /// Asynchronously retrieves a list of workflows formatted as options for a dropdown menu.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection
    /// of <see cref="OptionsConfig"/> objects, each representing a workflow option.
    /// </returns>
    public async Task<IEnumerable<OptionsConfig>> GetWorkflowListForDropdownAsync()
    {
        return await _sqlQuery.QueryReturningTypeAsync(
            new WorkflowListQueryForDropdownQuery(),
            "Get antibiotic list",
            new QueryFilterConfig()
        );
    }

    /// <summary>
    /// Writes a workflow document back to the configs table in a single transaction owned by the command.
    /// </summary>
    /// <param name="config">The workflow being saved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result describes what was written,
    /// including the identity and the name actually stored.
    /// </returns>
    public async Task<WorkflowDesignerSaveResultModel> SaveWorkflowDesignerConfigAsync(SaveWorkflowDesignerConfigModel config)
    {
        return await _sqlCommand.CommandWithTypeReturningTypeAsync(
            new SaveWorkflowDesignerConfigCommand(),
            "Save workflow designer configuration",
            config,
            _logWriter);
    }
}
