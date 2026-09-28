using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Represents the configuration for the Specime Archive list view.
    /// </summary>
    internal class ArchiveListViewConfig
    {
        /// <summary>
        /// Gets the view configuration for the Specime Archive list view.
        /// </summary>
        /// <returns>The list view configuration for the Specime Archive list view.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                name: 'archive',
                type: 'ManageList',
                title: '@SpeSpeP@',
                headerText: '@SpeManE@.',
                queryName: 'SpecimenArchiveList',
                singleQuery: 'singlespecimenforspecimenlist',
                multiselect: true,
                itemType: 'specimen',
                gridColumns:
                    [
                        { key: 'alert', name: '', fieldName: '', minWidth: 20, maxWidth: 20, isResizable: false, isCollapsible: false },
                        { key: 'turnaroundtime', name: '', fieldName: 'TurnAroundTimeColour', minWidth: 28, maxWidth: 28, isResizable: false },
                        { key: 'column1', name: '@SpeAcc@', fieldName: 'accessionnumber', minWidth: 135, maxWidth: 135, isResizable: true, isCollapsible: false, isSorted: false, isSortedDescending: false },
                        { key: 'menu', name: '', fieldName: '', minWidth: 130, maxWidth: 130, isResizable: true, isCollapsible: false },
                        { key: 'column2', name: '@PatFir@', fieldName: 'firstname', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column3', name: '@PatSurA@', fieldName: 'surname', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column4', name: '@PatRef@', fieldName: 'patientref', minWidth: 90, maxWidth: 90, isResizable: true, isCollapsible: false },
                        { key: 'column5', name: '@SpeSpeB@', fieldName: 'specimentype', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                        { key: 'column6', name: '@SpeColC@', fieldName: 'collectiondate', minWidth: 115, maxWidth: 115, isResizable: true, isCollapsible: true },
                        { key: 'column7', name: '@SpeRecD@', fieldName: 'receiveddate', minWidth: 110, maxWidth: 110, isResizable: true, isCollapsible: true },
                        { key: 'column8', name: '@GenSta@', fieldName: 'state', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false, highlight: true },
                        { key: 'column9', name: '@SpeMod@', fieldName: 'lastmodifieddate', minWidth: 110, maxWidth: 120, isResizable: true, isCollapsible: true, isSorted: true, isSortedDescending: true }
                    ],
                buttons:
                    [
                        { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewspecimenrecord' },
                        { key: 'diary', text: '@GenDiaA@', icon: 'DietPlanNotebook', onSelect: true, primaryAction: 11, uievent: 'specimendiaryuievent' },
                        { key: 'restartspecimen', text: '@GenResB@', icon: 'SwitcherStartEnd', onSelect: true, primaryAction: 2, uievent: 'restartspecimenuievent', onFinish: 'update' },
                        { key: 'Reports', text: '@GenVieD@', icon: 'ReportDocument', uievent: 'specimenreportuievent', workflow: false, onSelect: true, 
                            buttons: [
                                { key: 'specimenreport', text: '@RepSpe@', icon: 'ReportDocument', uievent: 'specimenreportuievent', workflow: false, onSelect: true}
                            ]
                        },
                        { key: 'publish', text: '@GenPubA@', icon: 'WebPublish', onBatch: true, uievent: 'batchpublishuievent', onFinish: 'refresh' }
                    ],
                filters: [
                    { key: 'state', placeholder: '@GenSta@', multiSelect: true, width: 200, optionsName: 'archivestatelist', fieldName: 'stateid' },
                    { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
                    { key: 'tag', placeholder: '@GenTagA@', multiSelect: true, width: 120, optionsName: 'Tag', fieldName: 'tagid' }
                ],
                filterSearch: true,
                searchFields: [ 'accessionnumber', 'firstname', 'surname', 'patientref', 'barcode', 'existingbarcode', 'specimentype', 'state' ]
            }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}
