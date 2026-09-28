using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Embedded list for instrument results on the test record view (direct or culture/isolate test).
/// Requires query parameters <c>id</c> (test row id) and <c>source</c> (direct or culture) from navigation <c>recordInfo</c>.
/// </summary>
internal class TestInstResultsListViewConfig
{
    /// <summary>
    /// Gets the view configuration for the test-scoped instrument results list.
    /// </summary>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'testinstresults',
                            'type': 'ManageList',
                            'title': '@InsIns@',
                            'headerText': '',
                            'queryName': 'TestInstrumentResultsQuery',
                            'parentId': 'id',
                            'addButton': 'requestinstrumenttestuievent',
                            'displaySummary': false,
                            'displayifempty': true,
                            'gridColumns': [
                                { 'key': 'column1', 'name': '@InsProNam@', 'fieldName': 'InstrumentProfile', 'minWidth': 160, 'maxWidth': 160, 'isResizable': true, isCollapsible: false },
                                { 'key': 'menu', 'name': '', 'fieldName': '', 'minWidth': 120, 'maxWidth': 120, 'isResizable': true, 'isCollapsible': false },
                                { 'key': 'column3', 'name': '@GenReq@', 'fieldName': 'RequestMade', 'minWidth': 160, 'maxWidth': 160, 'isResizable': true, isCollapsible: false },
                                { 'key': 'column4', 'name': '@GenStaA@', 'fieldName': 'Status', 'minWidth': 100, 'maxWidth': 100, 'isResizable': true, isCollapsible: true },
                                { 'key': 'column5', 'name': '@SpeRecH@', 'fieldName': 'ResultReceived', 'minWidth': 100, 'maxWidth': 100, 'isResizable': true, isCollapsible: false }
                            ],
                            'buttons': [
                                { 'key': 'viewinstrumentresultrecord', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, 'primaryAction': 1, 'uievent': 'viewinstrumentresultrecord', 'onFinish': 'update' }
                            ]
                        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
