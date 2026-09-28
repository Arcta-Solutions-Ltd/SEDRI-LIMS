using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Embedded list of requests on the admission record view.
/// </summary>
internal class AdmissionRequestListViewConfig
{
    /// <summary>
    /// Gets the list view configuration for requests linked to an admission.
    /// </summary>
    /// <returns>The list view configuration.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            name: 'admissionrequestslist',
                            type: 'ManageList',
                            title: '@NeoReq@',
                            headerText: '@NeoReq@.',
                            queryName: 'requestsforadmission',
                            parentId: 'AdmissionId',
                            gridColumns: [
                                { key: 'column1', name: '@NeoReqRef@', fieldName: 'requestid', minWidth: 120, maxWidth: 160, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'column2', name: '@NeoReqDat@', fieldName: 'requestdate', minWidth: 110, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'column3', name: '@NeoWar@', fieldName: 'ward', minWidth: 120, maxWidth: 180, isResizable: true, isCollapsible: false },
                                { key: 'column4', name: '@NeoUrg@', fieldName: 'urgency', minWidth: 100, maxWidth: 140, isResizable: true, isCollapsible: false },
                                { key: 'column5', name: '@NeoInd@', fieldName: 'indication', minWidth: 140, maxWidth: 220, isResizable: true, isCollapsible: false },
                                { key: 'column6', name: '@NeoSpeCnt@', fieldName: 'specimencount', minWidth: 90, maxWidth: 110, isResizable: true, isCollapsible: false }
                            ],
                            buttons: [
                                { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewrequestrecorduievent' },
                                { key: 'edit', text: '@GenEdiB@', icon: 'Edit', onSelect: true, primaryAction: 2, uievent: 'editrequestuievent', onFinish: 'embeddedrefresh' },
                                { key: 'delete', text: '@GenDelC@', icon: 'Delete', onSelect: true, primaryAction: 3, uievent: 'deleterequestuievent', onFinish: 'embeddedrefresh' }
                            ]
                    }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
