using arc.domain.Configuration.ViewConfig.ListViewConfig;

namespace arc.app.Config.Views.ListViews;

using Newtonsoft.Json;

/// <summary>
/// Configuration provider for the IQC tests list view.
/// </summary>
internal class IqcTestsViewConfig
{
    /// <summary>
    /// Gets the <see cref="ListViewConfig"/> for managing internal quality control tests.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> instance deserialized from the embedded JSON definition.
    /// </returns>
    internal ListViewConfig GetView()
    {
        const string view = @"{
            'name': 'iqctests',
            'type': 'ManageList',
            'title': '@QuaIqcTesA@',
            'workflow': 'iqctestworkflow',
            'headerText': '@QuaIqcTesDes@.',
            'singleQuery': 'singleiqctestforiqctestslistquery',
            'queryName': 'iqctestslist',
            'dateSearch': 'range',
            'gridColumns': [
                { 'key': 'column1', 'name': '@ConIde@', 'fieldName': 'accessionnumber', 'minWidth': 50,  'maxWidth': 200, 'isResizable': true },
                { 'key': 'menu',      'name': '',           'fieldName': '',               'minWidth': 130, 'maxWidth': 130, 'isResizable': true, 'isCollapsible': false },
                { 'key': 'column2',   'name': '@GenStaA@',  'fieldName': 'state',          'minWidth': 100, 'maxWidth': 200, 'isResizable': true },
                { 'key': 'column3',   'name': '@GenDatB@',  'fieldName': 'createddate',    'minWidth': 100, 'maxWidth': 200, 'isResizable': true },
                { 'key': 'column4',   'name': '@GenDatA@',  'fieldName': 'completeddate',  'minWidth': 100, 'maxWidth': 200, 'isResizable': true }
            ],
            'buttons': [
                { 'key': 'addtest',             'text': '@QuaAdd@',       'icon': 'Add',    'onSelect': false, 'uievent': 'addiqctestuievent',           'onFinish': 'refresh', 'workflow': false },
                { 'key': 'runtest',             'text': '@QuaEnt@',       'icon': 'Play',   'onSelect': true,  'primaryAction': 2, 'uievent': 'runiqctestuievent',           'onFinish': 'update',  'workflow': true  },
                { 'key': 'markiqctestcomplete', 'text': '@QuaMarCom@',    'icon': 'accept','onSelect': true,  'primaryAction': 3, 'uievent': 'markiqctestcompleteuievent', 'onFinish': 'update',  'workflow': true  },
                { 'key': 'viewtest',            'text': '@GenVieC@',      'icon': 'Redeye','onSelect': true,  'primaryAction': 1, 'uievent': 'viewiqctestuievent',         'onFinish': 'refresh' },
                { 'key': 'deletetest',          'text': '@GenDelC@',      'icon': 'Delete','onSelect': true,  'uievent': 'deleteiqctestuievent',       'onFinish': 'refresh' }
            ],
            'filterSearch': true,
            'filters': [
                {
                    'key': 'status',
                    'placeholder': '@GenStaA@',
                    'multiSelect': false,
                    'width': 140,
                    'optionsName': 'IqcTestState',
                    'fieldName': 'stateid'
                }
            ],
            'searchFields': ['accessionnumber', 'status']
        }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
