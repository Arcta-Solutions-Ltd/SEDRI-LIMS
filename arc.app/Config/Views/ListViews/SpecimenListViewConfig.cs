using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews
{
    /// <summary>
    /// Provides the configuration for the specimen list view.
    /// </summary>
    internal class SpecimenListViewConfig
    {
        /// <summary>
        /// Retrieves the configuration for the specimen list view.
        /// </summary>
        /// <returns>A <see cref="ListViewConfig"/> object representing the specimen list view configuration.</returns>
        internal ListViewConfig GetView()
        {
            var view = @"{
                            name: 'specimens',
                            type: 'ManageList',
                            title: '@SpeManD@',
                            headerText: '@SpeCreA@.',
                            queryName: 'SpecimenList',
                            workflow: 'SpecimenDefault',
                            multiselect: true,
                            singleQuery: 'singlespecimenforspecimenlist',
                            tests: ['cellcounttestuievent', 'gramstaintestuievent', 'indiainktestuievent', 'wetpreptestuievent', 'znstaintestuievent', 'auraminetestuievent',
                                    'biochemistrytestuievent','dipsticktestuievent','kohpreptestuievent','microscopytestuievent','pregnancytestuievent','wrightsstaintestuievent',
                                    'hpyloriantigentestuievent','jevserologytestuievent', 'esbltestuievent','betalactamasetestuievent','carbapenemasetestuievent', 'apipaneltestuievent',
                                    'gramculturetestuievent','oxidasetestuievent','catalasetestuievent'],
                            ""reports"": [
                                ""defaultspecimenreport""
                            ],
                            ""reportCategories"": [
                                {
                                    ""name"": ""Main"",
                                    ""dataSections"": ""SpecimenCoreDataSection,ApprovalDataSection,AuramineDataSection,BiochemistryDataSection,CellCountDataSection,DipstickDataSection,GramStainDataSection,HpyloriAntigenDataSection,ImageDataSection,IndiaInkDataSection,JevSerologyDataSection,KohPrepDataSection,LocationDataSection,MicroscopyDataSection,OxidaseDataSection,PatientDetailsDataSection,PregnancyDataSection,SpecimenCommentsDataSection,WetPrepDataSection,WrightsStainDataSection,ZnStainDataSection""
                                },
                                {
                                    ""name"": ""Organism"",
                                    ""dataSections"": ""ApiPanelDataSection,AstDataSection,BetalactamaseDataSection,BloodAndBottleWeightDataSection,CarbapenemaseDataSection,CatalaseDataSection,CultureCommentsDataSection,CultureDateDataSection,CultureResultDataSection,EsblDataSection,GramCultureDataSection,OrganismListDataSection,PrecultureResultsDataSection""
                                }
                            ],
                            ""allowedHeaders"": ""DefaultSpecimenReportHeader"",
                            ""allowedFooters"": ""DefaultSpecimenReportFooter"",
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
                                    { key: 'column6', name: '@SpeColE@', fieldName: 'collectiondate', minWidth: 75, maxWidth: 75, isResizable: true, isCollapsible: true },
                                    { key: 'column7', name: '@SpeRecH@', fieldName: 'receiveddate', minWidth: 75, maxWidth: 75, isResizable: true, isCollapsible: true },
                                    { key: 'column8', name: '@GenSta@', fieldName: 'state', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false, highlight: true },
                                    { key: 'column9', name: '@GenTagK@', fieldName: 'tags', minWidth: 150, maxWidth: 200, isResizable: true, isCollapsible: true, defaultHidden: true },
                                    { key: 'column10', name: '@SpeMod@', fieldName: 'lastmodifieddate', minWidth: 110, maxWidth: 120, isResizable: true, isCollapsible: true, isSorted: true, isSortedDescending: true }
                                ],
                            buttons:
                                [
                                    { key: 'addspecimenworkflow', text: '@SpeAddK@', icon: 'Add',
                                      buttons: [{key: 'workflow1', text: '@SpeAdv@', icon: 'Mail', uievent: 'createspecimenrequest', onFinish: 'refresh' },
                                                {key: 'workflow2', text: '@SpeAlr@', icon: 'TestBeakerSolid', uievent: 'createspecimenreceived', onFinish: 'refresh' },
                                                {key: 'workflow3', text: '@NeoNeo@', icon: 'Family', uievent: 'createneoshieldspecimen', onFinish: 'refresh' }]
                                    },
                                    { key: 'batchspecimen', text: '@SpeBatE@', icon: 'Add', onBatch: true,
                                      buttons: [{key: 'batchsubmit', text: '@SpeBatF@', onBatch: true, icon: 'Generate', uievent: 'batchsubmitconfirmationuievent', onFinish: 'refresh' },
                                                {key: 'batchapprovalone', text: '@SpeBatG@', onBatch: true, icon: 'DocumentApproval', uievent: 'batchspecimenapprovaloneuievent', onFinish: 'refresh' },
                                                {key: 'batchapprovaltwo', text: '@SpeBatH@', onBatch: true, icon: 'DocumentApproval', uievent: 'batchspecimenapprovaltwouievent', onFinish: 'refresh' },
                                                {key: 'batchaddtag', text: '@SpeBatI@', onBatch: true, icon: 'Tag', uievent: 'batchaddspecimentaguievent', onFinish: 'refresh' }]
                                    },
                                    { key: 'editspecimen', text: '@SpeEdiC@', icon: 'Edit', onSelect: true, primaryAction: 12, uievent: 'editspecimenuievent', onFinish: 'update', workflow: true },
                                    { key: 'view', text: '@GenVieC@', icon: 'RedEye', onSelect: true, primaryAction: 1, uievent: 'viewspecimenrecord', onFinish: 'update' },
                                    { key: 'ackreceipt', text: '@SpeAckA@', icon: 'ReceiptCheck', onSelect: true, primaryAction: 2, uievent: 'ackreceiptuievent', onFinish: 'update', workflow: true },
                                    { key: 'cancelrequest', text: '@SpeCanA@', icon: 'Cancel', onSelect: true, primaryAction: 3, uievent: 'specimencancelrequestuievent', onFinish: 'update', workflow: true },
                                    { key: 'rejectspecimen', text: '@SpeRejA@', icon: 'Cancel', onSelect: true, primaryAction: 10, uievent: 'rejectspecimen', onFinish: 'update', workflow: true },
                                    { key: 'testheader', text: '@SpeDir@', icon: 'TestExploreSolid', onSelect: true, primaryAction: 4, uievent: 'directtestuievent', workflow: true, onFinish: 'updateoncancel',
                                        buttons: [
                                            { key: 'testselection', text: '@GenSpeA@', icon: 'TestStep', uievent: 'testselectionuievent', workflow: true, onFinish: 'update' },
                                            { key: 'directtests', text: '@GenVieA@', icon: 'TestExploreSolid', uievent: 'directtestuievent', workflow: true, onFinish: 'updateoncancel' }
                                        ]
                                    },
                                    { key: 'addculture', text: '@SpeAddD@', icon: 'TestPlan', onSelect: true, uievent: 'addcultureuievent', primaryAction: 5, onFinish: 'update', workflow: true },
                                    { key: 'submit', text: '@GenSub@', icon: 'Generate', onSelect: true, primaryAction: 6, uievent: 'submitconfirmationuievent', onFinish: 'update', workflow: true },
                                    { key: 'diary', text: '@GenDiaA@', icon: 'DietPlanNotebook', onSelect: true, primaryAction: 11, uievent: 'specimendiaryuievent' },
                                    { key: 'approvalone', text: '@GenFir@', icon: 'DocumentApproval', onSelect: true, primaryAction: 7, uievent: 'specimenapprovaloneuievent', workflow: true, onFinish: 'update' },
                                    { key: 'approvaltwo', text: '@GenFirA@', icon: 'DocumentApproval', onSelect: true, primaryAction: 8, uievent: 'specimenapprovaltwouievent', workflow: true, onFinish: 'update' },
                                    { key: 'day0benchread', text: '@SpePro@', icon: 'RepeatAll', onSelect: true, primaryAction: 9, uievent: 'day0benchread', workflow: true, onFinish: 'update',
                                        rules : [
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '525'},
                                            { effect: 'visible', field: 'stateid', rule: '=', value: '526|527'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '528'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '529'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '530'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '531'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '532'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '533'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '534'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '535'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '537'}
                                        ]
                                    },
                                    { key: 'day1benchread', text: '@SpeDay@', icon: 'RepeatAll', onSelect: true, primaryAction: 10, uievent: 'day1benchread', workflow: true, onFinish: 'update',
                                        rules : [
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '525'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '526'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '527'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '528'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '529'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '530'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '531'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '532'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '533'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '534'},
                                            { effect: 'visible', field: 'stateid', rule: '=', value: '535'},
                                            { effect: 'visible', field: 'stateid', rule: '!=', value: '537'}
                                        ]
                                    },
                                    { key: 'movetopatient', text: '@PatMov@', icon: 'Merge', onSelect: true, primaryAction: 17, uievent: 'movepatientuievent', onFinish: 'update', workflow: false },
                                    { key: 'Reports', text: '@GenVieD@', icon: 'ReportDocument', uievent: 'specimenreportuievent', workflow: false, onSelect: true, primaryAction: 15 },
                                    { key: 'printbarcode1', text: '@GenPri@', icon: 'QRCode', onSelect: true, primaryAction: 13, uievent: 'printspecimenbarcode1uievent', workflow: false },
                                    { key: 'printbarcode2', text: '@GenPriA@', icon: 'QRCode', onSelect: true, primaryAction: 14, uievent: 'printspecimenbarcode2uievent', workflow: false },
                    			    { key: 'AddComment', text: '@GenAddC@', icon: 'CommentAdd', onSelect: true, primaryAction: 16, uievent: 'specimencommentuievent', workflow: false },
                    			    { key: 'addspecimentag', text: '@GenTagK@', icon: 'Tag', onSelect: true, primaryAction: 18, uievent: 'addspecimentaguievent', workflow: false, onFinish: 'update' },
                    			    { key: 'PrintPreview', text: '@GenPriF@', icon: 'Print', onSelect: true, primaryAction: 17, uievent: 'specimenprintpreviewuievent', workflow: false }
                                ],
                            filters: [
                                { key: 'rxddate', placeholder: '@SpeRecD@', multiSelect: true, width: 140, optionsName: 'ReceivedDate', fieldName: 'receiveddate' },
                                { key: 'state', placeholder: '@GenSta@', multiSelect: true, width: 200, optionsName: 'shortstatelist', fieldName: 'stateid' },
                                { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
                                { key: 'tag', placeholder: '@GenTagA@', multiSelect: true, width: 250, optionsName: 'Tag', fieldName: 'tagid', dropdownwidth: 850, type: 'hierarchicalpicker' }
                            ],
                            filterPresets: [
                                { key: 'filter3', name: '@SpeTod@', fields: [ { key: 'state', values: [ '526'] } ] },
                                { key: 'filter1', name: '@SpeDay@', fields: [
                                        { key: 'state', values: [ '535'] }
                                    ]
                                },
                                { key: 'filter2', name: '@AstPen@', fields: [ { key: 'state', values: [ '529'] } ] }
                            ],
                            filterSearch: true,
                            searchFields: [ 'accessionnumber', 'firstname', 'surname', 'patientref', 'barcode', 'existingbarcode' ]
                    }";

            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

            return result;
        }
    }
}


// Specimen States:
//
// 525 - "Pending Arrival"
// 526 - "Specimen Received"
// 527 - "Specimen Active"
// 528 - "Specimen Rejected"
// 529 - "Pending ID/AST"
// 530 - "Pending Approval Level 1"
// 531 - "Pending Approval Level 2"
// 532 - "Analysis Rejected Level 1"
// 533 - "Analysis Rejected Level 2"
// 534 - "Specimen Finalised"
// 535 - "Specimen Culturing"
// 537 - "Request Cancelled"

// Culture Types
//
// 978 - "Routine Culture"
// 979 - "Fungal Culture"
// 980 - "Anaerobic Culture"
// 981 - "Enrichment Culture"
// 982 - "Melioidosis Culture"
