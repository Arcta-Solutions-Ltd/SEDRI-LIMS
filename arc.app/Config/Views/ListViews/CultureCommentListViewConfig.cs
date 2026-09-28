using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Culture Comment list view.
    /// </summary>
    internal class CultureCommentListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Culture Comment list view.
        /// </summary>
        /// <returns>The list view configuration for the Culture Comment list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'culturecomments',
                            'type': 'ManageList',
                            'title': '@GenComE@',
                            'headerText': '@GenComE@.',
                            'queryName': 'CultureCommentListQuery',
                            'addbutton': 'culturecommentuievent',
                            'displaySummary': true,
                            'parentId': 'CultureId',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '', 'fieldName': '', 'minWidth': 20, 'maxWidth': 20, 'isResizable': true, isCollapsible: false },                               
                                { 'key': 'column2', 'name': '@GenComB@', 'fieldName': 'Comment', 'minWidth': 460, 'maxWidth': 550, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column3', 'name': '@GenComF@', 'fieldName': 'CommentType', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: true },  
                                { 'key': 'column4', 'name': '@GenMod@', 'fieldName': 'AddedBy', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column4', 'name': '@ExpProMd@', 'fieldName': 'LastModifiedDate', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column5', 'name': '@GenInc@', 'fieldName': 'DisplayOnReport', 'minWidth': 160, 'maxWidth': 160, 'isResizable': true, isCollapsible: true }
                            ],
                            'buttons': [
                                { key: 'edit', text: '@GenComG@', icon: 'Edit', onSelect: true, primaryAction: 1, uievent: 'editcommentuievent', onFinish: 'update' },
                                { key: 'delete', text: '@GenComN@', icon: 'Delete', onSelect: true, primaryAction: 2, uievent: 'deletecommentuievent', onFinish: 'update' }
                        ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}