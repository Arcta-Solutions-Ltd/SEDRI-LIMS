using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

internal class SpecimenCommentListViewConfig
{
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'specimencomments',
                            'type': 'ManageList',
                            'title': '@GenComE@',
                            'headerText': '@GenComE@.',
                            'queryName': 'CommentListBySpecimenId',
                            'addbutton': 'specimencommentuievent',
                            'workflow': 'SpecimenDefault', 
                            'displaySummary': true,
                            'displayifempty': false,
                            'parentId': 'SpecimenId',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '', 'fieldName': '', 'minWidth': 20, 'maxWidth': 20, 'isResizable': true, isCollapsible: false },                                
                                { 'key': 'column2', 'name': '@GenComB@', 'fieldName': 'Comment', 'minWidth': 460, 'maxWidth': 550, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column3', 'name': '@GenComF@', 'fieldName': 'CommentType', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column4', 'name': '@GenMod@', 'fieldName': 'AddedBy', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: true }, 
                                { 'key': 'column5', 'name': '@ExpProMd@', 'fieldName': 'LastModifiedDate', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column6', 'name': '@GenInc@', 'fieldName': 'DisplayOnReport', 'minWidth': 160, 'maxWidth': 160, 'isResizable': true, isCollapsible: true }
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
