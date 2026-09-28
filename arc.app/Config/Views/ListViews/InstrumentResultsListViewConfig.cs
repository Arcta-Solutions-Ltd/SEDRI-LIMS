using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// This class configures the view for the instrument results list in the application.
    /// </summary>
    internal class InstrumentResultsListViewConfig
    {
        /// <summary>
        /// Retrieves the configuration for the instrument results list view.
        /// </summary>
        /// <returns>A ListViewConfig object representing the instrument results list view configuration.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                            name: 'instrumentresults',
                            type: 'ManageList',
                            title: '@InsManB@',
                            headerText: '@InsManC@.',
                            queryName: 'InstrumentResultsListQuery',
                            singleQuery: 'singleitemininstrumentresultslist',
                            itemType: 'specimen',
                            workflow: 'InstrumentResultWorkflow', 
                            multiselect: true,
                            displaySummary: true,
                            dateSearch: 'range',
                            gridColumns:
                                [
                                    { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                    { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column2', name: '@GenPro@', fieldName: 'InstrumentProfile', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                    { key: 'column3', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column4', name: '@CulTyp@', fieldName: 'CultureType', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false},
                                    { key: 'column5', name: '@PatPatA@', fieldName: 'PatientName', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: true },
                                    { key: 'column6', name: '@InsManD@', fieldName: 'Barcode', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column7', name: '@GenStaA@', fieldName: 'Status', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column8', name: '@InsReq@', fieldName: 'RequestMade', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column9', name: '@InsRes@', fieldName: 'ResultReceived', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false }
                                ],
                            buttons:
                                [
                                    //{ key: 'acceptresults', text: '@InsAcc@', icon: 'ReceiptCheck', onSelect: true, primaryAction: 1, uievent: 'acceptresultsuievent', onFinish: 'refresh', workflow: true },
                                    //{ key: 'rejectresults', text: '@InsRej@', icon: 'Cancel', onSelect: true, primaryAction: 2, uievent: 'rejectresultsuievent', onFinish: 'refresh', workflow: true },
                                    //{ key: 'batchspecimen', text: '@SpeBatE@', icon: 'Add', onBatch: true,
                                    //  buttons: [
                                    //        { key: 'batchacceptresults', text: '@InsBatD@', icon: 'ReceiptCheck', uievent: 'batchacceptresultsuievent', onFinish: 'refresh', onBatch: true },
                                    //        { key: 'batchrejectresults', text: '@InsBatE@', icon: 'Cancel', uievent: 'batchrejectresultsuievent', onFinish: 'refresh', onBatch: true }
                                    //    ]
                                    //},
                                    { 'key': 'viewinstrumentresultrecord', 'text': '@GenVieC@', 'icon': 'RedEye', 'onSelect': true, 'primaryAction': 3, 'uievent': 'viewinstrumentresultrecord', 'onFinish': 'refresh' }
                                ],
                            filters: [
                                { key: 'specimentype', placeholder: '@SpeSpeB@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
                                { key: 'culturetype', placeholder: '@CulTyp@', multiSelect: true, width: 240, optionsName: 'CultureType', fieldName: 'culturetypeid' },
                                { key: 'resultstatus', placeholder: '@GenStaA@', multiSelect: true, width: 240, optionsName: 'InstrumentStatus', fieldName: 'statusid' }
                            ],
                            filterPresets: [
                                { key: 'filter3', name: '@InsAwa@', fields: [ { key: 'resultstatus', values: [ '885'] } ] },
                            ],
                            filterSearch: false
                    }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}


