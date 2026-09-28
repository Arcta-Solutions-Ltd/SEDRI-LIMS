using arc.app.Config;
using arc.common.Models;
using arc.common.Models.Lists;
using arc.domain.Configuration.ListsConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace arc.app.Common;

/// <summary>
/// Handler for processing dynamic list models into list configurations.
/// </summary>

public class HandleList : IHandleList
{
    private readonly IDataListFactory _dataListRepository;
    private readonly ILanguageHandler _languageHandler;
    private readonly ILogWriter _logWriter;


    /// <summary>
    /// Initializes a new instance of the <see cref="HandleList"/> class.
    /// </summary>
    /// <param name="dataListRepository">The repository for retrieving data lists.</param>
    /// <param name="languageHandler">The handler for translating list options.</param>
    /// <param name="logWriter">Logger for diagnostic messages on installed systems.</param>
    public HandleList(IDataListFactory dataListRepository, ILanguageHandler languageHandler, ILogWriter logWriter)
    {
        _dataListRepository = dataListRepository;
        _languageHandler = languageHandler;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Handles the conversion of dynamic list models to list configurations asynchronously.
    /// </summary>
    /// <param name="listModel">The dynamic list models to process.</param>
    /// <param name="token">The token containing user-specific information.</param>
    /// <returns>A list of <see cref="ListConfig"/> representing the processed list configurations.</returns>
    /// <remarks>
    /// This method iterates through the provided dynamic list models, retrieving and processing their configurations.
    /// If translation is required, it uses the <see cref="ILanguageHandler"/> to translate list options.
    /// Certain options are conditionally removed based on list IDs, ensuring proper filtering.
    /// </remarks>
    public async Task<List<ListConfig>> HandleAsync(List<DynamicListModel> listModel, TokenInfoModel token)
    {
        var returnList = new List<ListConfig>();
        foreach (var listItem in listModel)
        {
            var newConfig = await _dataListRepository.GetListAsync(listItem.Name.Trim(), listItem.IncludeFixed, token, listItem.ParentNodesOnly);

            if (listItem.Translate)
            {
                var optionList = await _languageHandler.TranslateAsync(JsonConvert.SerializeObject(newConfig.Options), token.LanguageId);
                newConfig.Options = JsonConvert.DeserializeObject<List<OptionsConfig>>(optionList).OrderBy(x => x.Text).ToList();
            }

            RemoveOptionsForCertainLists(listItem.Id, newConfig);

            if (newConfig.Options == null || newConfig.Options.Count == 0)
            {
                var contextId = string.IsNullOrWhiteSpace(listItem.Id) ? "(none)" : listItem.Id;
                var context = $"request Id={contextId}, includeFixed={listItem.IncludeFixed}, " +
                    $"translate={listItem.Translate}, parentNodesOnly={listItem.ParentNodesOnly}";

                if (listItem.ParentNodesOnly)
                {
                    // A parent-nodes-only request only ever comes from a self referencing table, so an empty
                    // result means the list has lost its internalhierarchy flag or its fixed parent rows. The
                    // combobox renders nothing when it has no options, so without this there is no visible clue.
                    _logWriter.LogError(
                        $"Self referencing list '{newConfig.Name}' returned no parent nodes, so the Parent field " +
                        $"will be missing from the table entry form ({context})",
                        nameof(HandleList),
                        nameof(HandleAsync));
                }
                else
                {
                    _logWriter.LogInfo(
                        $"Dynamic list '{newConfig.Name}' returned no options ({context})",
                        nameof(HandleList),
                        nameof(HandleAsync));
                }
            }

            returnList.Add(newConfig);
        }

        return returnList;
    }

    /// <summary>
    /// Removes specific options from a list configuration based on its ID.
    /// </summary>
    /// <param name="Id">The ID of the list to apply filtering.</param>
    /// <param name="listConfig">The list configuration to filter.</param>
    private void RemoveOptionsForCertainLists(string Id, ListConfig listConfig)
    {
        if (!string.IsNullOrWhiteSpace(Id))
        {
            if (Id.StartsWith("editorganisationform"))
            {
                listConfig.Options = listConfig.Options.Where(l => l.Key != "459").ToList();
            }
        }
    }
}
