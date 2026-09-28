using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Embedded specimen list on the request record view, scoped by <c>RequestId</c>.
/// </summary>
internal class RequestSpecimenListViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the request specimen list view.
    /// </summary>
    /// <returns>A <see cref="ListViewConfig"/> for specimens linked to the request.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            name: 'requestspecimenlist',
                            type: 'ManageList',
                            title: '@SpeManC@',
                            headerText: '@SpeCreA@.',
                            queryName: 'SpecimenListByRequestId',
                            workflow: 'SpecimenDefault', 
                            singleQuery: 'singlespecimenforspecimenlist',
                            tests: ['cellcounttestuievent', 'gramstaintestuievent', 'indiainktestuievent', 'wetpreptestuievent', 'znstaintestuievent', 'auraminetestuievent',
                                    'biochemistrytestuievent','dipsticktestuievent','kohpreptestuievent','microscopytestuievent','pregnancytestuievent','wrightsstaintestuievent',
                                    'hpyloriantigentestuievent','jevserologytestuievent', 'esbltestuievent','betalactamasetestuievent','carbapenemasetestuievent', 'apipaneltestuievent'],
                            parentId: 'RequestId',
                            gridColumns:
                                [
                                    { key: 'column1', name: '@SpeAcc@', fieldName: 'accessionnumber', minWidth: 140, maxWidth: 140, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: false },
                                    { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                    { key: 'column4', name: '@SpeSpeB@', fieldName: 'specimentype', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                                    { key: 'column6', name: '@SpeColC@', fieldName: 'collectiondate', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                                    { key: 'column7', name: '@SpeRecD@', fieldName: 'receiveddate', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                                    { key: 'column8', name: '@GenSta@', fieldName: 'state', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false, highlight: true }
                                ],
                            buttons:
                                [
                                    { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewspecimenrecord' }
                                ]
                    }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
