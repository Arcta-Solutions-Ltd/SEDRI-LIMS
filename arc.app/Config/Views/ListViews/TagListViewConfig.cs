using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Tags list view.
    /// Allows users to view, add, edit, and delete tags (ListItem where ListId=105).
    /// </summary>
    internal class TagListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Tags list view.
        /// </summary>
        /// <returns>The list view configuration for the Tags view.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'tagsconfig',
                            'type': 'ManageList',
                            'parentId': 'parenttagid',
                            'title': '@GenTagB@',
                            'headerText': '@GenTagB@.',
                            'queryName': 'TagList',
                            'singleQuery': 'SingleTagForTagList',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenNam@', 'fieldName': 'Value', 'minWidth': 300, 'maxWidth': 400, 'isResizable': true },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false }
                            ],
                            'buttons': [
                                { 'key': 'addtag', 'text': '@AleAddH@', 'icon': 'Add', 'onSelect': false, 'uievent': 'addtaguievent', 'onFinish': 'refresh' },
                                { 'key': 'addchildtag', 'text': '@AleAddChildTag@', 'icon': 'Add', 'onSelect': true, 'primaryAction': 1, 'uievent': 'addtaguievent', 'onFinish': 'refresh', addChildContext: true, prefillFormFields: { 'ParentTagId': 'id', 'ParentTag': 'Value' } },
                                { 'key': 'edittag', 'text': '@AleEdiG@', 'icon': 'Edit', 'onSelect': true, 'uievent': 'edittaguievent', 'onFinish': 'update', 'primaryAction': 2 },
                                { 'key': 'deletetag', 'text': '@AleDelH@', 'icon': 'Delete', 'uievent': 'deletetaguievent', 'onSelect': true, 'onFinish': 'refresh' }
                            ],
                            filterSearch: true,
                            searchFields: [ 'searchtext' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
