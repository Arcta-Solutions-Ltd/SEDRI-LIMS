using arc.app.Common;
using arc.common.Models.Config;
using arc.common.Models.Lists;
using arc.data.Common;
using arc.data.Utils;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// This class defines the ListRepository which inherits from GeneralRepository and implements IListRepository.
    /// </summary>
    /// <param name="sqlQuery">The ISqlQuery object for executing SQL queries.</param>
    /// <param name="logWriter">The ILogWriter object for logging information.</param>
    /// <param name="sqlCommand">The ISqlCommand object for executing SQL commands.</param>
    public class ListRepository(ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand)
        : GeneralRepository(sqlQuery, logWriter, sqlCommand), IListRepository
    {

        /// <summary>
        /// Retrieves list values asynchronously based on the provided list name and includeFixed flag.
        /// </summary>
        /// <param name="listName">The name of the list to retrieve values from.</param>
        /// <param name="includeFixed">A boolean flag to include fixed values.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of OptionsConfig representing the list values.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetListValuesAsync(string listName, bool includeFixed, bool parentNodesOnly = false)
        {
            var queryFilters = new QueryFilterConfig
            {
                Parameters =
                [
                    new() { Value = listName },
                    new() { Value = includeFixed.ToString() },
                    new() { Value = parentNodesOnly.ToString() }
                ]
            };

            _logWriter.LogInfo("Run Get List Values Query", "ListRepository", "GetListValuesAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetListValuesQuery(), "Get List values", queryFilters);
        }

        /// <summary>
        /// Retrieves the list of tags (ListItem where ListId=105) for the tags list view.
        /// </summary>
        /// <param name="queryFilters">Filter configuration; supports searchText for filtering.</param>
        /// <returns>A list of tag models with Id and Value.</returns>
        public async Task<List<TagListModel>> GetTagListForViewAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run Tag List Query", "ListRepository", "GetTagListForViewAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new TagListQuery(), "Get tag list for view", queryFilters);
        }

        /// <summary>
        /// Retrieves a single tag by Id for edit/delete forms.
        /// </summary>
        /// <param name="queryFilters">Filter configuration; expects 'id' parameter.</param>
        /// <returns>The tag model if found.</returns>
        public async Task<TagListModel> GetSingleTagForTagListAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run Single Tag For Tag List Query", "ListRepository", "GetSingleTagForTagListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new SingleTagForTagListQuery(), "Get single tag for tag list", queryFilters);
        }

        /// <summary>
        /// Retrieves list values by ID asynchronously based on the provided query filters.
        /// </summary>
        /// <param name="id">The ID of the list to retrieve values from.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of OptionsConfig representing the list values.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetListValuesByIdAsync(int id)
        {
            var queryFilters = new QueryFilterConfig
            {
                Parameters =
                [
                    new() { Key = "id", Value = id.ToString() }
                ]
            };

            _logWriter.LogInfo("Run Get List Values by id Query", "ListRepository", "GetListValuesByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetListValuesByIdQuery(), "Get List values by id", queryFilters);
        }

        /// <summary>
        /// Retrieves the child list items of a parent list item in a hierarchical list.
        /// </summary>
        /// <param name="parentId">The list item id whose children are wanted, for example a workflow's StatesList.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the child items as options, keyed by list item id.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetListValuesByParentIdAsync(int parentId)
        {
            var queryFilters = new QueryFilterConfig().AddInteger("parentid", parentId);

            _logWriter.LogInfo($"Run Get List Values by parent id Query for parent {parentId}", "ListRepository", "GetListValuesByParentIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetListValuesByParentIdQuery(), "Get List values by parent id", queryFilters);
        }

        /// <summary>
        /// Retrieves a common list asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of OptionsConfig representing the common list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetCommonListAsync()
        {
            _logWriter.LogInfo("Run Get Common List Query", "ListRepository", "GetCommonListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new CommonListQuery(), "Get common list", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves a custom list asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of OptionsConfig representing the custom list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetCustomListAsync()
        {
            _logWriter.LogInfo("Run Get Custom List Query", "ListRepository", "GetCustomListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new CustomListQuery(), "Get custom list", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves a short state list asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of OptionsConfig representing the short state list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetShortStateListAsync()
        {
            _logWriter.LogInfo("Run Get Short State List Query", "ListRepository", "GetShortStateListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ShortStateListQuery(), "Get short state list", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves a state list asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of OptionsConfig representing the state list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetStateListAsync()
        {
            _logWriter.LogInfo("Run Get State List Query", "ListRepository", "GetStateListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new StateListQuery(), "Get state list", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves the list ID from a value asynchronously.
        /// </summary>
        /// <param name="value">The value to search for.</param>
        /// <param name="listId">The list ID to search within.</param>
        /// <param name="parentId">The parent ID to search within (optional).</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the list ID as an integer.</returns>
        public async Task<int> GetListIdFromValueAsync(string value, int listId, int parentId = 0)
        {
            var queryFilters = new QueryFilterConfig()
                .AddString("Value", value)
                .AddString("ListId", listId.ToString())
                .AddString("ParentId", parentId.ToString());

            _logWriter.LogInfo("Run Get ListId from Value Query", "ListRepository", "GetListIdFromValue");
            return await _sqlQuery.QueryReturningIntegerAsync(new GetListIdFromValueQuery(), "Get List Id from Value", queryFilters);
        }

        /// <summary>
        /// Retrieves a value from an ID asynchronously.
        /// </summary>
        /// <param name="id">The ID to retrieve the value from.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the retrieved value as a string.</returns>
        public async Task<string> GetValueFromIdAsync(int id)
        {
            var queryFilters = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "Id", Value = id.ToString() } } };

            _logWriter.LogInfo("Run Value from Id Query", "ListRepository", "GetValueFromId");
            return await _sqlQuery.QueryReturningStringAsync(new GetListValueFromIdQuery(), "Get List values ", queryFilters);
        }

        /// <summary>
        /// Retrieves list contents asynchronously based on the provided query filters.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an enumerable of ListContentsModel objects.</returns>
        public async Task<IEnumerable<ListContentsModel>> GetListContentsAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run Get List Contents Query", "ListRepository", "GetListContentsAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ListContentsQuery(), "Get List Contents", queryFilters);
        }

        /// <summary>
        /// Retrieves list contents by ID asynchronously based on the provided query filters.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListContentsModel object.</returns>
        public async Task<ListContentsModel> GetListContentsByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run List Contents Query", "ListRepository", "GetListContentsByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ListContentsByIdQuery(), "Get List Contents by ListId", queryFilters);
        }

        /// <summary>
        /// Retrieves a list by ID asynchronously based on the provided query filters.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListByIdQueryModel object.</returns>
        public async Task<ListByIdQueryModel> GetListByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run List By Id Query", "ListRepository", "GetListByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ListByIdQuery(), "Get List by ListId", queryFilters);
        }

        /// <summary>
        /// Returns all lists that reference a parent list in the table hierarchy.
        /// </summary>
        /// <returns>Child list rows with parent list id and name.</returns>
        public async Task<List<ChildListInfoModel>> GetChildListHierarchyAsync()
        {
            _logWriter.LogInfo("Run Child List Hierarchy Query", "ListRepository", "GetChildListHierarchyAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ChildListHierarchyQuery(), "Get child list hierarchy", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves a list by value asynchronously based on the provided query filters.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListByIdQueryModel object.</returns>
        public async Task<ListByIdQueryModel> GetListByValueAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run List By Value Query", "ListRepository", "GetListByValueAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ListByValueQuery(), "Get List by Value", queryFilters);
        }

        /// <summary>
        /// Retrieves a list item by its ID asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListItemModel object.</returns>
        public async Task<ListItemModel> GetListItemByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run Get List Item By Id Query", "ListRepository", "GetListItemByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ListItemByIdQuery(), "Get List Item by Id", queryFilters);
        }

        /// <summary>
        /// Retrieves a list item by its value asynchronously.
        /// </summary>
        /// <param name="queryFilters">The QueryFilterConfig object containing query filters.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains the ListItemModel object.</returns>
        public async Task<ListItemModel> GetListItemByValueAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run Get List Item By Value Query", "ListRepository", "GetListItemByValueAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ListItemByValueQuery(), "Get List Item by Value", queryFilters);
        }

        /// <summary>
        /// Adds a table entry asynchronously.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an integer representing the result of the command.</returns>
        public async Task<int> AddTableEntryAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<ListItemModel>(dataToSave);
            _logWriter.LogInfo($"Run Add Table Entry Command ListId={data.ListId} ParentId={data.ParentId}", "ListRepository", "AddTableEntryAsync");
            return await _sqlCommand.CarryOutCommandReturningIntegerAsync(new AddTableEntryCommand(), "Insert table entry", data.Value, data.ListId.ToString(), data.ParentId?.ToString(), data.Enabled, data.Fixed);
        }

        /// <summary>
        /// Adds a table asynchronously.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an integer representing the result of the command.</returns>
        public async Task<int> AddTableAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<AddTableModel>(dataToSave);
            _logWriter.LogInfo("Run Add Table Command", "ListRepository", "AddTableAsync");
            return await _sqlCommand.CarryOutCommandReturningIntegerAsync(new AddTableCommand(), "Insert table", data.Name, data.Description, data.ParentId.ToString());
        }

        /// <summary>
        /// Edits a table entry asynchronously.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result contains an integer representing the result of the command.</returns>
        public async Task<int> EditTableEntryAsync(string dataToSave)
        {
            var data = JsonConvert.DeserializeObject<ListItemModel>(dataToSave);
            _logWriter.LogInfo("Run Edit Table Entry Command", "ListRepository", "EditTableEntryAsync");
            return await _sqlCommand.CarryOutCommandReturningIntegerAsync(new EditTableEntryCommand(), "Edit table entry", data.Value, data.Id.ToString(), data.ParentId.ToString(), data.Enabled);
        }

        /// <summary>
        /// Deletes a table entry asynchronously based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the table entry to be deleted.</param>
        public async Task DeleteTableEntryAsync(string id)
        {
            _logWriter.LogInfo("Run Delete Table Entry Command", "ListRepository", "DeleteTableEntryAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteTableEntryCommand(), "Delete table entry", id);
        }

        /// <summary>
        /// Deletes a table asynchronously based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the table to be deleted.</param>
        public async Task DeleteTableAsync(string id)
        {
            _logWriter.LogInfo("Run Delete Table Command", "ListRepository", "DeleteTableAsync");
            await _sqlCommand.CarryOutCommandAsync(new DeleteTableCommand(), "Delete table", id);
        }

        /// <summary>
        /// Orders the list based on the provided field list asynchronously.
        /// </summary>
        /// <param name="fieldList">The list of fields to order.</param>
        /// <returns>An integer representing the result of the command.</returns>
        public async Task<int> OrderListCommandAsync(List<FieldListModel> fieldList)
        {
            _logWriter.LogInfo("Order List Command", "ListRepository", "OrderListCommandAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new OrderListCommand(), "Order list command", fieldList);
        }

        /// <summary>
        /// Retrieves the archive state list asynchronously.
        /// </summary>
        /// <returns>An enumerable of OptionsConfig representing the archive state list.</returns>
        public async Task<IEnumerable<OptionsConfig>> GetArchiveStateListAsync()
        {
            _logWriter.LogInfo("Run Get Archive State List Query", "ListRepository", "GetArchiveStateListAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new ArchiveStateListQuery(), "Get archive state list", new QueryFilterConfig());
        }

        /// <summary>
        /// Retrieves the tag hierarchy for specified tag ID(s) asynchronously.
        /// Expands each tag to include itself and all descendants in the hierarchy.
        /// </summary>
        /// <param name="commaSeparatedTagIds">Comma-separated tag IDs to expand.</param>
        /// <returns>Comma-separated string of tag IDs (selected tags plus all descendants).</returns>
        public async Task<string> GetTagHierarchyAsync(string commaSeparatedTagIds)
        {
            if (string.IsNullOrWhiteSpace(commaSeparatedTagIds))
            {
                return "";
            }

            var queryFilters = new QueryFilterConfig
            {
                Parameters =
                [
                    new QueryValuesConfig { Key = "tagid", Value = commaSeparatedTagIds }
                ]
            };

            _logWriter.LogInfo("Run Tag Hierarchy Query", "ListRepository", "GetTagHierarchyAsync");
            return await _sqlQuery.QueryReturningStringAsync(new TagHierarchyListQuery(), "Get Tag Hierarchy Query", queryFilters);
        }
    }
}
