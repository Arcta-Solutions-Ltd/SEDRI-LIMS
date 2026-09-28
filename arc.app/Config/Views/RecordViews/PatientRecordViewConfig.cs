using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews
{
    internal class PatientRecordViewConfig
    {
        internal RecordViewConfig GetView()
        {

            var view = @"{
                            'title': '@PatPatC@',
                            'name': 'patientrecordview',
                            'type': 'recordview',
                            'singleItemName': '@SinPat@',
                            'buttons': [
                                { key: 'edit', text: '@PatEdiA@', icon: 'Edit', uievent: 'editpatientuievent', onFinish: 'update' },
                                { key: 'delete', text: '@PatDel@', icon: 'Delete', uievent: 'deletepatientuievent' },
                                { key: 'merge', text: '@PatMer@', icon: 'Merge', uievent: 'mergepatientuievent', onFinish: 'update' },
                                { key: 'diary', text: '@GenDiaA@', icon: 'DietPlanNotebook', uievent: 'patientdiaryuievent' },
                                { key: 'printbarcode1', text: '@GenPri@', icon: 'QRCode', uievent: 'printpatientbarcode1uievent' },
                                { key: 'printbarcode2', text: '@GenPriA@', icon: 'QRCode', uievent: 'printpatientbarcode2uievent' },
                                { key: 'AddComment', text: '@GenAddC@', icon: 'CommentAdd', uievent: 'patientcommentuievent', onFinish: 'update' },
                                { key: 'addpatienttag', text: '@GenTagK@', icon: 'Tag', uievent: 'addpatienttaguievent', onFinish: 'refresh' },
                                { key: 'manageattachments', text: '@GenAtts@', icon: 'Attach', uievent: 'managepatientattachmentsuievent', onFinish: 'refresh' }
                            ],
                            'regions': [
                                { 'id': 'patientdetails',
                                  'type': 'standard',
                                  'queryName': 'patientforpatientview'
                                },
                                { 'id': 'admissions',
                                  'type': 'listview',
                                  'title': '@NeoAdm@',
                                  'listViewName': 'patientadmissionslist'
                                },
                                { 'id': 'requests',
                                  'type': 'listview',
                                  'title': '@NeoReq@',
                                  'listViewName': 'patientrequestslist'
                                },
                                { 'id': 'specimens',
                                  'type': 'listview',
                                  'title': '@GenSpe@',
                                  'listViewName': 'patientspecimenlist'
                                },
                                { 'id': 'attachments', 'type': 'standard', 'queryName': 'patientattachmentsforpatientview', 'title': '@GenAtts@' },
                                { 'id': 'patientcomments',
                                  'type': 'listview',
                                  'title': '@GenComE@',
                                  'listViewName': 'patientcomments'
                                },
                                { 'id': 'reports',
                                  'type': 'crafted',
                                  'title': '@RepRep@',
                                  'Name': 'reporthistorygrid',
                                  'queryName': 'patientreportlist'
                                }
                            ]
                         }";

            var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

            return result;
        }
    }
}
