import {PostList} from '../../Data/Post';

const mapFilterOptions = (options, optionsName, filterType) => {
    const mapped = options.map((option) => ({
        key: option.Key ?? option.key,
        text: option.Text ?? option.text ?? '',
        ParentKey: option.ParentKey ?? option.parentkey ?? option.parentKey
    }));
    const type = (filterType && String(filterType).toLowerCase()) || '';
    const isTag = optionsName && optionsName.toLowerCase() === 'tag';
    if (type === 'hierarchicalpicker' || isTag) {
        return mapped;
    }
    return mapped.map((o) => ({ key: o.key, text: o.text }));
};

const AddListsIntoFilters = (filters, lists) => {

    let returnFilters = [];
   
    if (filters !== undefined && Array.isArray(filters)) {
        for (const filter of filters) {
            returnFilters.push({...filter});
        }
        for (const filter of returnFilters) {
            const optionsName = filter.OptionsName || filter.optionsName;
            if (optionsName !== undefined) {
                const listName = (optionsName || '').toLowerCase();
                const list = lists.filter(l => (l.Name || l.name || '').toLowerCase() === listName);
                if (list.length > 0) {
                    const listOptions = list[0].Options || list[0].options || [];
                    const filterType = (filter.Type || filter.type || '').toLowerCase();
                    const isTag = (optionsName || '').toLowerCase() === 'tag';
                    const hierarchical = mapFilterOptions(listOptions, optionsName, filter.Type || filter.type);
                    filter.Options = (filterType === 'hierarchicalpicker' || isTag)
                        ? hierarchical
                        : hierarchical.map((o) => ({ key: o.key, text: o.text }));
                }
            }
        } 
    }

    return returnFilters;
}

const AddDynamicFilterList = (filters, lists) => {

    var dynamicFilters = filters.filter(f => f.Dynamic);
    for (const filter of dynamicFilters) {
        const optionsName = filter.OptionsName || filter.optionsName;
        if (optionsName !== undefined) {
            const list = lists.filter(l => (l.name || l.Name || '').toLowerCase() === optionsName.toLowerCase());
            if (list.length > 0) {
                const listOptions = list[0].options || list[0].Options || [];
                const options = listOptions.map((option) => ({
                    key: option.key ?? option.Key,
                    text: option.text ?? option.Text ?? '',
                    ParentKey: option.parentkey ?? option.ParentKey ?? option.parentKey
                }));
                const filterType = (filter.Type || filter.type || '').toLowerCase();
                const isTag = optionsName.toLowerCase() === 'tag';
                filter.Options = filterType === 'hierarchicalpicker' || isTag
                    ? options
                    : options.map((o) => ({ key: o.key, text: o.text }));
            }
        }
    }
    return dynamicFilters;
}

const DoesFilterContainDynamicList = (filters) => {
    var dynamicFilters = filters.filter(f => f.Dynamic);
    return dynamicFilters.length > 0;
}

const GetDynamicListsFromDatabase = (state, retrievedHandler, errorWhenRetrievingData ) => {
    let dynamicLists = [];
    
    if (DoesFilterContainDynamicList(state.filters)) {

        var dynamicFilters = state.filters.filter(f => f.Dynamic);
        for (const filter of dynamicFilters) {
            //dynamicLists = dynamicLists === "" ? filter.OptionsName : dynamicLists + "," + filter.OptionsName;
            dynamicLists.push({name: filter.OptionsName, includeFixed: filter.IncludeFixed === undefined ? true : filter.IncludeFixed});
        }
        PostList(dynamicLists, retrievedHandler, errorWhenRetrievingData, {...state});
    }
}

export default AddListsIntoFilters;
export { AddDynamicFilterList,DoesFilterContainDynamicList,GetDynamicListsFromDatabase }