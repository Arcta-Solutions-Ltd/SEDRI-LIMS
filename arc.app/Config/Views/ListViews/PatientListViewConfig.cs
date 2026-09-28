using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// This class configures the view for the patient list in the application.
    /// </summary>
    internal class PatientListViewConfig
    {
        /// <summary>
        /// Retrieves the configuration for the patient list view.
        /// </summary>
        /// <returns>A ListViewConfig object representing the patient list view configuration.</returns>
        internal ListViewConfig GetView()
        {
            // JSON string that represents the configuration for the patient list view.
            var view = @"{
                        name: 'patients',
                        type: 'ManageList',
                        title: '@PatMan@',
                        headerText: '@PatCre@.',
                        queryName: 'PatientList',
                        multiselect: true,
                        singleQuery: 'SinglePatientForPatientList',
                        recordView: 'patientrecordview',
                        gridColumns: [
                            { key: 'column1', name: '@PatFir@', fieldName: 'firstname', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false },
                            { key: 'column12', name: '@PatSurA@', fieldName: 'surname', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: false },
                            { key: 'menu', name: '', fieldName: '', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: false },
                            { key: 'column2', name: '@PatDat@', fieldName: 'dateofbirth', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                            { key: 'column3', name: '@PatGenA@', fieldName: 'gender', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                            { key: 'column4', name: '@PatPatB@', fieldName: 'patientref', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                            { key: 'column5', name: '@GenLoc@', fieldName: 'fullyqualifiedname', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
                            { key: 'column9', name: '@GenTagK@', fieldName: 'tags', minWidth: 150, maxWidth: 200, isResizable: true, isCollapsible: true, defaultHidden: true }
                        ],
                        buttons: [
                            { key: 'batchpatient', text: '@SpeBatE@', icon: 'Add', onBatch: true,
                                  buttons: [{key: 'batchaddtag', text: '@SpeBatI@', onBatch: true, icon: 'Tag', uievent: 'batchaddpatienttaguievent', onFinish: 'refresh' }]
                                    },
                            { key: 'view', text: '@PatVie@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewpatientrecorduievent' },
                            { key: 'edit', text: '@PatEdiA@', icon: 'Edit', onSelect: true, primaryAction: 2, uievent: 'editpatientuievent', onFinish: 'update' },
                            { key: 'addspecimenforpatientworkflow', text: '@SpeAddK@', icon: 'Add', 'onSelect': true,
                                  buttons: [{key: 'workflow1forpatient', text: '@SpeAdv@', icon: 'Mail', uievent: 'createspecimenrequestforpatientuievent', onFinish: 'refresh' },
                                            {key: 'workflow2forpatient', text: '@SpeAlr@', icon: 'TestBeakerSolid', uievent: 'createspecimenreceivedforpatientuievent', onFinish: 'refresh' },
                                            {key: 'workflow3forpatient', text: '@NeoNeo@', icon: 'Family', uievent: 'createneoshieldspecimenforpatientuievent', onFinish: 'refresh' }]
                                },
                            { key: 'diary', text: '@GenDiaA@', icon: 'DietPlanNotebook', onSelect: true, primaryAction: 3, uievent: 'patientdiaryuievent' },
                            //{ key: 'delete', text: '@PatDel@', icon: 'Delete', onSelect: true, uievent: 'deletepatientuievent', onFinish: 'refresh' },
                            { key: 'merge', text: '@PatMer@', icon: 'Merge', onSelect: true, uievent: 'mergepatientuievent', onFinish: 'refresh' },
                            { key: 'printbarcode1', text: '@GenPri@', icon: 'QRCode', onSelect: true, primaryAction: 12, uievent: 'printpatientbarcode1uievent' },
                            { key: 'printbarcode2', text: '@GenPriA@', icon: 'QRCode', onSelect: true, primaryAction: 13, uievent: 'printpatientbarcode2uievent' },
                            { key: 'AddComment', text: '@GenAddC@', icon: 'CommentAdd', onSelect: true, primaryAction: 14, uievent: 'patientcommentuievent' }
                        ],
                        filters: [
                            { key: 'location', placeholder: '@GenLoc@', width: 160, optionsName: 'LocationList', fieldName: 'LocationId', type: 'hierarchicalpicker', multiSelect: true, dropdownwidth: 300, allowAdd: false },
                            { key: 'gender', placeholder: '@PatGenA@', multiSelect: true, width: 160, optionsName: 'Gender', fieldName: 'GenderId' },
                            { key: 'tag', placeholder: '@GenTagA@', multiSelect: true, width: 250, optionsName: 'Tag', fieldName: 'tagid', dropdownwidth: 850, type: 'hierarchicalpicker' }
                        ],
                        filterSearch: true,
                        dateSearch: true,
                        dateSearchLabel: '@PatDat@',
                        searchFields: [ 'firstName', 'surname', 'barcode', 'patientref', 'fullyqualifiedname', 'gender' ]
                    }";

            // Deserialize the JSON string into a ListViewConfig object.
            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            // Return the ListViewConfig object.
            return result;
        }
    }

}
