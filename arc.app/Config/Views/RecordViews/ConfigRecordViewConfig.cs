using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the system configuration record view.
/// This view provides a central interface for managing various system configurations including
/// forms, tests, reports, and workflows.
/// </summary>
internal class ConfigRecordViewConfig
{
    /// <summary>
    /// Gets the record view configuration for system configuration management.
    /// This view contains multiple list view regions for different configuration types.
    /// </summary>
    /// <returns>A RecordViewConfig instance containing the view definition.</returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                            'title': '@GenSys@',
                            'name': 'views',
                            'type': 'recordview',
                            buttons:
                                [
                                ],
                            'regions': [
                                { 'id': 'forms',
                                    'type': 'listview',
                                    'title': '@GenFor@',
                                    'listViewName': 'formconfig'
                                },
                                { 'id': 'directtests',
                                  'type': 'listview',
                                  'title': '@ConDirA@',
                                  'listViewName': 'directtestconfig'
                                },
                                { 'id': 'culturetests',
                                  'type': 'listview',
                                  'title': '@SpeCulC@',
                                  'listViewName': 'culturetestconfig'
                                },
                                { 'id': 'reportlist',
                                   'type': 'listview',
                                   'title': '@GenVieD@',
                                   'listViewName': 'reportlistconfig'
                                },
                                { 'id': 'workflowlist',
                                   'type': 'listview',
                                   'title': '@GenWor@',
                                   'listViewName': 'workflows'
                                }
                            ]
                         }";


        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}


// { 'id': 'reportlist',
//   'type': 'listview',
//   'title': '@GenVieD@',
//   'listViewName': 'reportlistconfig'
// },
// { 'id': 'workflowsteps',
//   'type': 'listview',
//   'title': '@ConWor@',
//   'listViewName': 'workflowstep'
// },
// { 'id': 'forms',
// 'type': 'listview',
//   'title': '@GenFor@',
//   'listViewName': 'formconfig'
//}




//Need to keep Ian

//var view = @"{
//                            'title': '@GenSys@',
//                            'name': 'views',
//                            'type': 'recordview',
//                            buttons:
//                                [
//                                    { key: 'stateheader', text: '@ConWorA@', icon: 'ProgressLoopInner', primaryAction: 4, uievent: 'addstateuievent', workflow: true,
//                                        buttons: [
//                                            { key: 'addstate', text: '@ConAddS@', icon: 'Add', uievent: 'addstateuievent'},
//                                            { key: 'editstate', text: '@ConEdiS@', icon: 'Edit', uievent: 'editstateuievent'},
//                                            { key: 'deletestate', text: '@ConDelU@', icon: 'Edit', uievent: 'deletestateuievent'}
//                                        ]
//                                    },
//                                ],
//                            'regions': [
//                                { 'id': 'forms',
//                                  'type': 'listview',
//                                  'title': '@GenFor@',
//                                  'listViewName': 'formconfig'
//                                },
//                                { 'id': 'directtests',
//                                  'type': 'listview',
//                                  'title': '@ConDirA@',
//                                  'listViewName': 'directtestconfig'
//                                },
//                                { 'id': 'culturetests',
//                                  'type': 'listview',
//                                  'title': '@SpeCulC@',
//                                  'listViewName': 'culturetestconfig'
//                                },
//                                { 'id': 'specimentypedirecttestmapping',
//                                  'type': 'listview',
//                                  'title': '@ConSpeA@',
//                                  'listViewName': 'specimentypedirecttest'
//                                },
//                                { 'id': 'specimentypeculturetypemapping',
//                                  'type': 'listview',
//                                  'title': '@ConSpe@',
//                                  'listViewName': 'specimentypeculturetype'
//                                },
//                                { 'id': 'reportlist',
//                                  'type': 'listview',
//                                  'title': '@GenVieD@',
//                                  'listViewName': 'reportlistconfig'
//                                },
//                                { 'id': 'workflowsteps',
//                                  'type': 'listview',
//                                  'title': '@GenWor@',
//                                  'listViewName': 'workflowstep'
//                                }
//                            ]
//                         }";
