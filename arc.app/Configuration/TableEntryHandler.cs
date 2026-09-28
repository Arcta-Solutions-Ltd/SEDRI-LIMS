using arc.app.Common;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Models.Lists;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Configuration
{
    /// <summary>
    /// Builds the crafted page payloads for the Add and Edit Table Entry forms used by Table Maintenance.
    /// The contents are emitted as a list of Key/value pairs because the UI form shell discards crafted
    /// contents that are not a list, which silently removes every field from the form.
    /// </summary>
    public class TableEntryHandler : ITableEntryHandler
    {
        private readonly IListRepository _listRepository;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Creates an instance of the table entry handler.
        /// </summary>
        /// <param name="listRepository">Repository used to read the list and list item being maintained.</param>
        /// <param name="logWriter">Logger used to record how the parent field was resolved on installed systems.</param>
        public TableEntryHandler(IListRepository listRepository, ILogWriter logWriter)
        {
            _listRepository = listRepository;
            _logWriter = logWriter;
        }

        /// <summary>
        /// Builds the Add Table Entry form payload for the list identified by the query filters.
        /// </summary>
        /// <param name="queryFilters">Query filters carrying the metaflistid of the list being added to.</param>
        /// <returns>Serialized crafted pages payload for the tableentrypage.</returns>
        public async Task<string> AddTableEntryAsync(QueryFilterConfig queryFilters)
        {
            var listDetails = await _listRepository.GetListByIdAsync(queryFilters);
            listDetails.Enabled = "Yes";

            LogParentResolution(listDetails, nameof(AddTableEntryAsync));

            return BuildCraftedPayload("tableentrypage", listDetails.ToCraftedContents());
        }

        /// <summary>
        /// Builds the Edit Table Entry form payload for the list item identified by the query filters.
        /// </summary>
        /// <param name="queryFilters">Query filters carrying the metaflistid of the list and the id of the entry.</param>
        /// <returns>Serialized crafted pages payload for the edittableentrypage.</returns>
        public async Task<string> EditTableEntryAsync(QueryFilterConfig queryFilters)
        {
            var listEntryDetails = await _listRepository.GetListByIdAsync(queryFilters);
            var listItemDetails = await _listRepository.GetListItemByIdAsync(queryFilters);

            var returnDetails = new ListItemWithOptionModel
            {
                Id = listItemDetails.Id,
                Value = listItemDetails.Value,
                Enabled = listItemDetails.Enabled,
                Fixed = listItemDetails.Fixed,
                ParentId = listItemDetails.ParentId,
                ListId = listItemDetails.ListId,
                OptionName = listEntryDetails.OptionName,
                InternalHierarchy = listEntryDetails.InternalHierarchy,
                InternalHierarchyParentOptionName = listEntryDetails.InternalHierarchyParentOptionName
            };

            LogParentResolution(listEntryDetails, nameof(EditTableEntryAsync));
            _logWriter.LogInfo(
                $"Edit table entry {returnDetails.Id} on list {returnDetails.ListId}: fixed={returnDetails.Fixed}, parentIds='{returnDetails.ParentId}'",
                nameof(TableEntryHandler),
                nameof(EditTableEntryAsync));

            return BuildCraftedPayload("edittableentrypage", returnDetails.ToCraftedContents());
        }

        /// <summary>
        /// Wraps crafted contents in the payload shape the UI expects for a form made up only of crafted pages.
        /// </summary>
        /// <param name="pageName">Name of the crafted page the contents belong to.</param>
        /// <param name="contents">Key/value pairs the crafted page reads its fields from.</param>
        /// <returns>Serialized crafted pages payload.</returns>
        private static string BuildCraftedPayload(string pageName, List<JsonKeyValuePairModel> contents)
        {
            var craftedModels = new List<CraftedModel>
            {
                new() { Name = pageName, Contents = JsonConvert.SerializeObject(contents) }
            };

            return JsonConvert.SerializeObject(new JustCraftedPages { Crafted = craftedModels });
        }

        /// <summary>
        /// Records which parent option source the list resolved to, and reports the states in which the form
        /// will open without a Parent field. Installed systems cannot be run in debug mode, so this is the
        /// only way to tell a table that has no parent from one that has lost its parent configuration.
        /// </summary>
        /// <param name="list">The list whose parent configuration was resolved.</param>
        /// <param name="method">Calling method name, for the log entry.</param>
        private void LogParentResolution(ListByIdQueryModel list, string method)
        {
            _logWriter.LogInfo(
                $"Table maintenance list {list.Id} '{list.Name}': internalHierarchy={list.InternalHierarchy}, " +
                $"parentListId={list.ParentListId?.ToString() ?? "(none)"}, optionName='{list.OptionName}', " +
                $"internalHierarchyParentOptionName='{list.InternalHierarchyParentOptionName}'",
                nameof(TableEntryHandler),
                method);

            if (list.InternalHierarchy && string.IsNullOrWhiteSpace(list.InternalHierarchyParentOptionName))
            {
                _logWriter.LogError(
                    $"List {list.Id} '{list.Name}' is flagged as a self referencing table but no parent option name " +
                    "was resolved, so the Parent field will be missing from the table entry form",
                    nameof(TableEntryHandler),
                    method);
            }

            if (list.IsChildTable() && string.IsNullOrWhiteSpace(list.OptionName))
            {
                _logWriter.LogError(
                    $"List {list.Id} '{list.Name}' references parent list {list.ParentListId} but that list could not " +
                    "be resolved, so the Parent field will be missing from the table entry form",
                    nameof(TableEntryHandler),
                    method);
            }
        }
    }
}
