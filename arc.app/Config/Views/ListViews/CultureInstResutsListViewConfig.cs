using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Culture Instrument Results list view.
    /// </summary>
    internal class CultureInstResultsListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Culture Instrument Results list view.
        /// </summary>
        /// <returns>The list view configuration for the Culture Instrument Results list view.</returns>
        internal static ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'cultureinstresults',
                            'type': 'ManageList',
                            'title': '@InsIns@',
                            'headerText': '',
                            'queryName': 'CultureInstrumentResults',
                            'parentId': 'CultureId',
                            'addButton': 'requestinstrumenttestuievent',
                            'displaySummary': false,
                            'displayIfEmpty': true,
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

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}

