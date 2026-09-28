using arc.common.Models.Config;
using arc.common.Models.SystemConfig;
using arc.data.model.Configuration;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.SystemConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.SystemConfig;

/// <summary>
/// Defines the contract for a configuration repository that handles operations related to configuration
/// data such as retrieval, insertion, update, and deletion of configuration items.
/// </summary>
public interface IConfigRepository
{
    /// <summary>
    /// Retrieves a list of configuration models based on the provided filter parameters.
    /// </summary>
    /// <param name="queryFilters">The filter criteria to apply to the configuration query.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="ConfigsModel"/>.
    /// </returns>
    Task<IEnumerable<ConfigsModel>> GetConfigListAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves a single configuration model using the configuration name as a filter.
    /// </summary>
    /// <param name="queryFilters">The filter criteria containing the configuration name.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="ConfigsModel"/> that matches the provided name.
    /// </returns>
    Task<ConfigsModel> SingleConfigByNameAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves a single configuration data model using the configuration ID as a filter.
    /// </summary>
    /// <param name="queryFilters">The filter criteria containing the configuration ID.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="ConfigsDataModel"/> that matches the provided ID.
    /// </returns>
    Task<ConfigsDataModel> SingleConfigByIdAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Adds a new custom configuration entry.
    /// </summary>
    /// <param name="dataToSave">A JSON string representing the configuration data to save.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the identifier of the newly created configuration entry.
    /// </returns>
    Task<int> AddCustomEntryAsync(string dataToSave);

    /// <summary>
    /// Edits an existing custom configuration entry.
    /// </summary>
    /// <param name="data">An instance of <see cref="ConfigsDataModel"/> containing the updated configuration data.</param>
    /// <returns>A task that represents the asynchronous edit operation.</returns>
    Task EditCustomEntryAsync(ConfigsDataModel data);

    /// <summary>
    /// Updates an existing custom configuration entry.
    /// </summary>
    /// <param name="data">An instance of <see cref="ConfigsDataModel"/> containing the updated configuration data.</param>
    /// <returns>A task that represents the asynchronous update operation.</returns>
    Task UpdateCustomEntryAsync(ConfigsDataModel data);

    /// <summary>
    /// Retrieves the next available configuration name based on the provided filters.
    /// </summary>
    /// <param name="queryFilters">The filter criteria to determine the next available configuration name.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is the next available configuration name as a string.
    /// </returns>
    Task<string> GetNextAvailableNameAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Deletes a custom configuration entry identified by its name.
    /// </summary>
    /// <param name="name">The name of the configuration entry to be deleted.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    Task DeleteCustomEntryAsync(string name);

    /// <summary>
    /// Retrieves a list of all configuration data models.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="ConfigsDataModel"/>.
    /// </returns>
    Task<IEnumerable<ConfigsDataModel>> AllConfigListAsync();



        Task<string> GetNextAccessionNumberAsync(string prefix, string seedMask, int numberLength, string category);


    /// <summary>
    /// Retrieves a complete list of name items.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="NameList"/>.
    /// </returns>
    Task<IEnumerable<NameList>> AllNameListAsync();

    /// <summary>
    /// Imports configuration data from a JSON string.
    /// </summary>
    /// <param name="dataToSave">A JSON string containing the configuration data to be imported.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an integer status code or identifier indicating the result of the import.
    /// </returns>
    Task<int> ImportConfigurationAsync(string dataToSave);

    /// <summary>
    /// Retrieves a list of mapping list view models based on the provided query filters.
    /// </summary>
    /// <param name="queryFilters">The filter criteria to apply to the mapping list view query.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of <see cref="MappingListViewModel"/>.
    /// </returns>
    Task<List<MappingListViewModel>> GetMappingListAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves a list of workflow models.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection of <see cref="WorkflowListModel"/>.
    /// </returns>
    Task<IEnumerable<WorkflowListModel>> GetWorkflowListAsync();

    /// <summary>
    /// Asynchronously retrieves a list of workflows formatted as options for a dropdown menu.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains an enumerable collection
    /// of <see cref="OptionsConfig"/> objects, each representing a workflow option.
    /// </returns>
    Task<IEnumerable<OptionsConfig>> GetWorkflowListForDropdownAsync();

    /// <summary>
    /// Writes a workflow document back to the configs table in a single transaction, matching on the
    /// configs identity first and only falling back to the name scoped to the workflow ConfigType.
    /// </summary>
    /// <param name="config">The workflow being saved.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result describes what was written,
    /// including the identity and the name actually stored.
    /// </returns>
    Task<WorkflowDesignerSaveResultModel> SaveWorkflowDesignerConfigAsync(SaveWorkflowDesignerConfigModel config);
}
