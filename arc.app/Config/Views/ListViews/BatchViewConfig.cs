//using arc.domain.Configuration.ViewConfig.ListViewConfig;
//using Newtonsoft.Json;

//namespace arc.app.Config.Views.ListViews
//{
//    /// <summary>
//    /// Represents the configuration for the Batch Report list view.
//    /// </summary>
//    internal class BatchViewConfig
//    {
//        /// <summary>
//        /// Gets the view configuration for the Batch Report list view.
//        /// </summary>
//        /// <returns>The list view configuration for the Batch Report list view.</returns>
//        internal static ListViewConfig GetView()
//        {
//            var view = @"{
//                            name: 'batch',
//                            type: 'MultiList',
//                            title: '@GenRepB@',
//                            headerText: '@BatPro@.',
//                            queryName: 'SpecimenBatchList',
//                            workflow: 'SpecimenDefault',
//                            itemType: 'specimen',
//                            filterSearch: false,
//                            multiselect: true,
//                            dateSearch: 'range',
//                            gridColumns:
//                                [
//                                    { key: 'column1', name: '@SpeAcc@', fieldName: 'AccessionNumber', minWidth: 140, maxWidth: 140, isResizable: true, isCollapsible: false, isSorted: true, isSortedDescending: true },
//                                    { key: 'column2', name: '@PatSurA@', fieldName: 'Surname', minWidth: 100, maxWidth: 100, isResizable: true, isCollapsible: false },
//                                    { key: 'column3', name: '@SpeSpeB@', fieldName: 'SpecimenType', minWidth: 180, maxWidth: 180, isResizable: true, isCollapsible: false },
//                                    { key: 'column4', name: '@GenLocA@', fieldName: 'OrganisationName', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true },
//                                    { key: 'column5', name: '@GenSta@', fieldName: 'State', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false, highlight: true },
//                                    { key: 'column6', name: '@GenDatC@', fieldName: 'DateFinalised', minWidth: 150, maxWidth: 150, isResizable: true, isCollapsible: false },
//                                    { key: 'column7', name: '@GenPub@', fieldName: 'Published', minWidth: 80, maxWidth: 80, isResizable: true, isCollapsible: true },
//                                    { key: 'column8', name: '@GenPriB@', fieldName: 'Printed', minWidth: 120, maxWidth: 120, isResizable: true, isCollapsible: true }
//                                ],
//                            buttons:
//                                [
//                                    { key: 'publish', text: '@GenPubA@', icon: 'WebPublish', onSelect: true, uievent: 'batchpublishuievent', onFinish: 'refresh' }
//                                ],
//                            filters: [
//                                { key: 'report', placeholder: '@GenRepA@', multiSelect: false, width: 140, optionsName: 'ReportList', fieldName: 'reportid' },
//                                { key: 'specimentype', placeholder: '@GenTyp@', multiSelect: true, width: 240, optionsName: 'SpecimenType', fieldName: 'specimentypeid' },
//                                { key: 'organisation', placeholder: '@GenLocA@', multiSelect: true, width: 200, optionsName: 'organisationlist', fieldName: 'organisationid', dynamic: true },
//                                { key: 'printed', placeholder: '@GenPriB@', multiSelect: true, width: 200, optionsName: 'printstatus', fieldName: 'printstatusid' },
//                            ],
//                            filterPresets: [
//                                { key: 'specimenreport', name: '@RepSpe@', default: true, fields: [ { key: 'report', values: [ '868'] } ] }
//                            ],
//                            searchFields: [ 'accessionnumber', 'surname' ]
//                    }";

//            var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

//            return result;
//        }
//    }
//}
