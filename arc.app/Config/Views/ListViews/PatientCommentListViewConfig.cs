using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class PatientCommentListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'patientcomments',
                            'type': 'ManageList',
                            'title': '@GenComE@',
                            'headerText': '@GenComE@.',
                            'queryName': 'CommentListByPatientId',
                            'displaySummary': true,
                            'parentId': 'PatientId',
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@GenComB@', 'fieldName': 'comment', 'minWidth': 710, 'maxWidth': 710, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@GenDat@', 'fieldName': 'lastmodifieddate', 'minWidth': 180, 'maxWidth': 180, 'isResizable': true, isCollapsible: true }
                            ],
                            'buttons': []
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
