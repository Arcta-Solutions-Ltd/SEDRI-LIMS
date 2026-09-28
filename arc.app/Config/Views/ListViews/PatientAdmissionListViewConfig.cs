using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Embedded list of admissions on the patient record view.
/// </summary>
internal class PatientAdmissionListViewConfig
{
    /// <summary>
    /// Gets the list view configuration for patient admissions.
    /// </summary>
    /// <returns>The list view configuration.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            name: 'patientadmissionslist',
                            type: 'ManageList',
                            title: '@NeoAdm@',
                            headerText: '@NeoAdm@.',
                            queryName: 'admissionsforpatient',
                            parentId: 'PatientId',
                            gridColumns: [
                                { key: 'column1', name: '@NeoAdmDat@', fieldName: 'admissiondate', minWidth: 120, maxWidth: 140, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                                { key: 'column2', name: '@NeoAdmTim@', fieldName: 'admissiontime', minWidth: 100, maxWidth: 120, isResizable: true, isCollapsible: false },
                                { key: 'column3', name: '@NeoReqCnt@', fieldName: 'requestcount', minWidth: 90, maxWidth: 110, isResizable: true, isCollapsible: false },
                                { key: 'column4', name: '@NeoSpeCnt@', fieldName: 'specimencount', minWidth: 90, maxWidth: 110, isResizable: true, isCollapsible: false }
                            ],
                            buttons: [
                                { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewadmissionrecorduievent' },
                                { key: 'edit', text: '@GenEdiB@', icon: 'Edit', onSelect: true, primaryAction: 2, uievent: 'editadmissionuievent', onFinish: 'embeddedrefresh' },
                                { key: 'delete', text: '@GenDelC@', icon: 'Delete', onSelect: true, primaryAction: 3, uievent: 'deleteadmissionuievent', onFinish: 'embeddedrefresh' }
                            ]
                    }";

        return JsonConvert.DeserializeObject<ListViewConfig>(view);
    }
}
