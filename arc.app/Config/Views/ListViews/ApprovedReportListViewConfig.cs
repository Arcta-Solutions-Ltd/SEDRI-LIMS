using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Provides configuration for the approved reports list view, including layout, filters, and actions.
/// </summary>
internal class ApprovedReportListViewConfig
{
    /// <summary>
    /// Builds and returns a configured <see cref="ListViewConfig"/> instance for displaying approved reports.
    /// </summary>
    /// <returns>A deserialized <see cref="ListViewConfig"/> object that defines the view layout and behavior.</returns>
    internal static ListViewConfig GetView()
    {
        var view = @"{
                            name: 'approvedreportview',
                            type: 'managelist',
                            title: '@RepRepC@',
                            headerText: '@RepVie@.',
                            queryName: 'ApprovedReportsListQuery',
                            itemType: 'reporthistory',
                            recordview: 'specimenreportview',
                            filterSearch: true,
                            multiselect: true,
                            dateSearch: 'range',
                            gridColumns:
                                [
                                    { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 140, maxWidth: 140, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
                                    { key: 'column2', name: '@PatSurA@', fieldName: 'Surname', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false },
                                    { key: 'column3', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 180, maxWidth: 180, isResizable: true, isCollapsible: false },
                                    { key: 'column4', name: '@GenLocA@', fieldName: 'OrganisationName', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                                    { key: 'column5', name: '@GenAppB@', fieldName: 'ApprovedBy', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                                    { key: 'column6', name: '@RepAppF@', fieldName: 'ApprovalDate', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false }
                                ],
                            buttons:
                                [
                                    { key: 'showreport', text: '@RepSho@', icon: 'RedEye', onSelect: true, primaryAction: 17, uievent: 'showreportonscreenuievent', workflow: false },
                                    { key: 'print', text: '@GenPriC@', icon: 'Print', onSelect: true, uievent: 'batchprintuievent', onBatch: true},
                                ],
                            filters: [
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
