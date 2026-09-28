//using arc.domain.Configuration.ViewConfig.ListViewConfig;
//using Newtonsoft.Json;

//namespace arc.app.Config.Views.ListViews
//{
//    internal class SpecimenBatchProcessingListViewConfig
//    {
//        internal ListViewConfig GetView()
//        {
//            var view = @"{
//                            name: 'specimenbatch',
//                            type: 'MultiList',
//                            title: '@GenRepB@',
//                            headerText: '@BatPro@.',
//                            queryName: 'SpecimenBatchList',
//                            workflow: 'SpecimenDefault',
//                            itemType: 'specimen',
//                            filterSearch: false,
//                            gridColumns:
//                                [
//                                    { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 140, maxWidth: 140, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
//                                    { key: 'column2', name: '@PatSurA@', fieldName: 'Surname', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false },
//                                    { key: 'column3', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 180, maxWidth: 180, isResizable: true, isCollapsible: false },
//                                    { key: 'column4', name: '@GenWar@', fieldName: 'OrganisationName', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
//                                    { key: 'column5', name: '@GenSta@', fieldName: 'State', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false, highlight: true },
//                                    { key: 'column6', name: '@GenDatC@', fieldName: 'DateFinalised', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false },
//                                    { key: 'column7', name: '@GenPub@', fieldName: 'Published', minWidth: 80, maxWidth: 80, isResizable: true, isCollapsible: true },
//                                    { key: 'column8', name: '@GenPriB@', fieldName: 'Printed', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true }
//                                ],
//                            buttons:
//                                [
//                                    { key: 'submit', text: '@GenSub@', icon: 'Generate', onSelect: true, uievent: 'submitconfirmationuievent', onFinish: 'update', workflow: true },
//                                    { key: 'approvalone', text: '@GenFir@', icon: 'DocumentApproval', onSelect: true, uievent: 'specimenapprovaloneuievent', workflow: true, onFinish: 'update' },
//                                    { key: 'approvaltwo', text: '@GenFirA@', icon: 'DocumentApproval', onSelect: true, uievent: 'specimenapprovaltwouievent', workflow: true, onFinish: 'update' },

//                                ],
//                            filters: [
//                                { key: 'state', placeholder: '@GenSta@', multiSelect: false, width: 200, optionsName: 'shortstatelist', fieldName: 'stateid' },
//                                { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
//                                { key: 'culturetype', placeholder: '@CulTyp@', multiSelect: true, width: 200, optionsName: 'culturetype', fieldName: 'culturetypeid' },
//                                { key: 'growth', placeholder: '@GenQua@', multiSelect: true, width: 200, optionsName: 'specimenquantity', fieldName: 'growthid' }
//                            ],
//                            filterPresets: [

//                            ],
//                            searchFields: [ 'accessionnumber', 'surname' ]
//                    }";

//            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

//            return result;
//        }
//    }
//}
