using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    internal class MonitoringListViewConfig
    {
        internal ListViewConfig GetView()
        {
            var view = @"{
                            'name': 'monitoring',
                            'type': 'ManageList',
                            'title': '@MonMon@',
                            'headerText': '@MonMonA@.',
                            'queryName': 'QueueList',
                            'gridColumns': [
                                { 'key': 'validity', 'name': '', 'fieldName': 'isValid', 'minWidth': 40, 'maxWidth': 40, 'isResizable': false },
                                { 'key': 'column1', 'name': '@GenEve@', 'fieldName': 'event', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true },
                                { 'key': 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { 'key': 'column2', 'name': '@SpeAcc@', 'fieldName': 'accessionnumber', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column3', 'name': '@PatPatB@', 'fieldName': 'patientref', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column4', 'name': '@GenTop@', 'fieldName': 'topic', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column5', 'name': '@GenStaA@', 'fieldName': 'eventstatus', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column6', 'name': '@GenUseA@', 'fieldName': 'username', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true },
                                { 'key': 'column7', 'name': '@GenAddA@', 'fieldName': 'added', 'minWidth': 130, 'maxWidth': 130, 'isResizable': true }
                            ],
                            'buttons': [
                                { 'key': 'viewdetails', 'text': '@MonVieA@', 'icon': 'Copy', 'onSelect': true, uievent: 'monitoringjsonviewer' },
                                { 'key': 'viewdetailsraw', 'text': '@MonVieB@', 'icon': 'RedEye', 'onSelect': true, uievent: 'vieweventdetails' }
                            ],
                            filters: [
                                { key: 'topic', placeholder: '@GenTop@', multiSelect: true, width: 140, optionsName: 'Topic', fieldName: 'topicid' },
                                { key: 'event', placeholder: '@GenEveA@', multiSelect: true, width: 200, optionsName: 'Event', fieldName: 'eventid' },
                                { key: 'status', placeholder: '@GenStaA@', multiSelect: true, width: 240, optionsName: 'EventStatus', fieldName: 'eventstatusid' }
                            ],
                            filterSearch: true,
                            searchFields: [ 'username', 'accessionnumber','patientref', 'topic', 'eventstatus', 'event' ]
                        }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
