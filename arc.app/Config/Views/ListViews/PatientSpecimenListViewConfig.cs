using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// This class represents the configuration for the patient specimen list view.
    /// </summary>
    internal class PatientSpecimenListViewConfig
    {
        /// <summary>
        /// Retrieves the configuration for the patient specimen list view.
        /// </summary>
        /// <returns>A ListViewConfig object representing the patient specimen list view configuration.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                            name: 'patientspecimenlist',
                            type: 'ManageList',
                            title: '@SpeManC@',
                            headerText: '@SpeCreA@.',
                            queryName: 'SpecimenListByPatientId',
                            workflow: 'SpecimenDefault', 
                            singleQuery: 'singlespecimenforspecimenlist',
                            tests: ['cellcounttestuievent', 'gramstaintestuievent', 'indiainktestuievent', 'wetpreptestuievent', 'znstaintestuievent', 'auraminetestuievent',
                                    'biochemistrytestuievent','dipsticktestuievent','kohpreptestuievent','microscopytestuievent','pregnancytestuievent','wrightsstaintestuievent',
                                    'hpyloriantigentestuievent','jevserologytestuievent', 'esbltestuievent','betalactamasetestuievent','carbapenemasetestuievent', 'apipaneltestuievent'],
                            parentId: 'PatientId',
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

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}

