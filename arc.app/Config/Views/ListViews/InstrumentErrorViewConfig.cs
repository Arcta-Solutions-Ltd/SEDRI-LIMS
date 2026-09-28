using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Represents the configuration for the Instrument Error list view.
/// </summary>
internal class InstrumentErrorViewConfig
{
    /// <summary>
    /// Gets the view configuration for the Instrument Error list view.
    /// </summary>
    /// <returns>The list view configuration for the Instrument Error list view.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            name: 'instrumenterror',
                            type: 'ManageList',
                            title: '@InsErr@',
                            headerText: '@InsLis@.',
                            queryName: 'InstrumentErrorListQuery',
                            itemType: 'specimen',
                            workflow: 'InstrumentErrorWorkflow', 
                            displaySummary: false,
                            multiselect: false,
                            gridColumns:
                                [
                                    { key: 'column1', name: '@GenPro@', fieldName: 'profilename', minWidth: 50, maxWidth: 100, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                    { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column2', name: '@GenStaA@', fieldName: 'errorstatus', minWidth: 50, maxWidth: 100, isResizable: true, isCollapsible: false },
                                    { key: 'column3', name: '@InsDir@', fieldName: 'instrumentdirection', minWidth: 50, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column4', name: '@GenErr@', fieldName: 'errortext', minWidth: 130, maxWidth: 250, isResizable: true, isCollapsible: false },
                                    { 'key': 'column5', 'name': '@ExpProMd@', 'fieldName': 'lastmodifieddate', 'minWidth': 150, 'maxWidth': 150, 'isResizable': true }
                                ],
                            buttons:
                                [
                                { 'key': 'viewinstrumenterrordetails', 'text': '@MonVieB@', 'icon': 'RedEye', 'onSelect': true, uievent: 'viewinstrumenterrordetailsuievent', primaryAction: 1 },
                                //{ 'key': 'replay', 'text': '@GenRepC@', 'icon': 'ChevronRight', 'onSelect': true, uievent: 'replayinstrumenterroruievent', onFinish: 'update', primaryAction: 2, workflow: true },
                                //{ 'key': 'batchreplay', text: '@InsBat@', onBatch: true, icon: 'DoubleChevronLeftMedMirrored', uievent: 'batchreplayinstrumenterroruievent', onFinish: 'refresh', workflow: true }
                                ],
                            filters: [
                                { key: 'errorstatus', placeholder: '@GenStaA@', multiSelect: false, width: 240, optionsName: 'InstrumentErrorStatus', fieldName: 'instrumentresultid' },
                                { key: 'instrumentdirection', placeholder: '@InsDir@', multiSelect: false, width: 240, optionsName: 'InstrumentDirection', fieldName: 'instrumentdirectionid' },
                                { key: 'profilelist', placeholder: '@GenPro@', multiSelect: false, width: 240, optionsName: 'ProfileList', fieldName: 'profilename' }
                            ],
                            filterSearch: true,
                            searchFields: [ 'profilename','errorstatus']
                    }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
