using arc.common.Models.Config;
using arc.common.Models.Lists;
using arc.data.model.Lists;
using arc.domain.Coding;
using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    public interface IListRepository : IGeneralRepository
    {
        Task<IEnumerable<OptionsConfig>> GetListValuesAsync(string listName, bool includeFixed, bool parentNodesOnly = false);
        Task<int> GetListIdFromValueAsync(string value, int listId, int parentId = 0);
        Task<string> GetValueFromIdAsync(int id);
        Task<IEnumerable<OptionsConfig>> GetCommonListAsync();
        Task<IEnumerable<ListContentsModel>> GetListContentsAsync(QueryFilterConfig queryFilters);
        Task<ListByIdQueryModel> GetListByIdAsync(QueryFilterConfig queryFilters);
        Task<List<ChildListInfoModel>> GetChildListHierarchyAsync();
        Task<ListItemModel> GetListItemByIdAsync(QueryFilterConfig queryFilters);
        Task<int> AddTableEntryAsync(string dataToSave);
        Task<int> AddTableAsync(string dataToSave);
        Task<int> EditTableEntryAsync(string dataToSave);
        Task<ListItemModel> GetListItemByValueAsync(QueryFilterConfig queryFilters);
        Task<ListContentsModel> GetListContentsByIdAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetShortStateListAsync();
        Task<IEnumerable<OptionsConfig>> GetStateListAsync();
        Task<ListByIdQueryModel> GetListByValueAsync(QueryFilterConfig queryFilters);
        Task<IEnumerable<OptionsConfig>> GetCustomListAsync();
        Task<IEnumerable<OptionsConfig>> GetListValuesByIdAsync(int id);

        /// <summary>
        /// Retrieves the child list items of a parent list item in a hierarchical list, for example the
        /// states a workflow's StatesList points at.
        /// </summary>
        /// <param name="parentId">The list item id whose children are wanted.</param>
        /// <returns>The child items as options, keyed by list item id.</returns>
        Task<IEnumerable<OptionsConfig>> GetListValuesByParentIdAsync(int parentId);
        Task<int> OrderListCommandAsync(List<FieldListModel> fieldList);
        Task DeleteTableEntryAsync(string dataToSave);
        Task DeleteTableAsync(string dataToSave);
        Task<IEnumerable<OptionsConfig>> GetArchiveStateListAsync();
        Task<List<TagListModel>> GetTagListForViewAsync(QueryFilterConfig queryFilters);
        Task<TagListModel> GetSingleTagForTagListAsync(QueryFilterConfig queryFilters);
        Task<string> GetTagHierarchyAsync(string commaSeparatedTagIds);
        //Task<IEnumerable<ListModel>> AllListsAsync();
        //Task<IEnumerable<ListItemDataModel>> AllListItemsAsync();
    }
}
