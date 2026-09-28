using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Provides the configuration for the Unapproved Reports list view.
/// </summary>
internal class UnapprovedReportListViewConfig
{
    /// <summary>
    /// Builds and returns a <see cref="ListViewConfig"/> for the unapproved reports view.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> instance deserialized from the embedded JSON definition.
    /// </returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
            name: 'unapprovedreportview',
            type: 'managelist',
            title: '@RepRepD@',
            headerText: '@RepVieA@.',
            workflow: 'unapprovedreportworkflow',
            queryName: 'UnapprovedReportsListQuery',
            singleQuery: 'UnapprovedReportsListByIdQuery',
            itemType: 'reporthistory',
            multiselect: true,
            filterSearch: true,
            dateSearch: 'range',
            recordview: 'specimenreportview',
            gridColumns:
                [
                    { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 140, maxWidth: 140, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                    { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                    { key: 'column2', name: '@PatSurA@', fieldName: 'Surname', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false },
                    { key: 'column3', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 180, maxWidth: 180, isResizable: true, isCollapsible: false },
                    { key: 'column4', name: '@GenLocA@', fieldName: 'OrganisationName', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                    { key: 'column5', name: '@GenReq@', fieldName: 'RequestedDate', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false },
                    { key: 'column6', name: '@GenAppC@', fieldName: 'Approved', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false },
                    { key: 'column7', name: '@GenAppB@', fieldName: 'ApprovedBy', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                    { key: 'column8', name: '@SpeAppD@', fieldName: 'ApprovalDate', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false }
                ],
            buttons:
                [
                    //{ key: 'viewreport', text: '@GenVie@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewreportuievent', onFinish: 'refresh' },
                    { key: 'showreport', text: '@RepSho@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'showreportonscreenuievent', workflow: false },
                    { key: 'approvereport', text: '@RepAppC@', icon: 'DocumentApproval', onSelect: true, primaryAction: 2, uievent: 'approvereportuievent', onFinish: 'update', workflow: true },
                    { key: 'unapprovereport', text: '@RepUnaB@', icon: 'PageRemove', onSelect: true, primaryAction: 3, uievent: 'unapprovereportuievent', onFinish: 'update', workflow: true },
                    { key: 'batchapprovereport', text: '@RepBatB@', onBatch: true, icon: 'DocumentApproval', uievent: 'batchapprovereportuievent', onFinish: 'refresh' },
                    { key: 'batchrejectreport', text: '@RepBatC@', onBatch: true, icon: 'DocumentApproval', uievent: 'batchrejectreportuievent', onFinish: 'refresh' }
                 ],
            filters: [
                { key: 'approved', placeholder: '@GenAppC@', multiSelect: true, width: 240, optionsName: 'ReportApproval', fieldName: 'approvedid' },
                { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
                { key: 'organisation', placeholder: '@GenLocA@', multiSelect: true, width: 200, optionsName: 'OrganisationList', fieldName: 'organisationfilterid', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false }
            ],
            filterPresets: [
            ],
            searchFields: [ 'accessionnumber', 'surname' ]
        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);
        return result;
    }
}
