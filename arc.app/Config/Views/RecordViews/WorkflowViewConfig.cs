using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Represents the configuration for displaying a workflow view. 
/// This view configuration is expressed as a JSON string and deserialized into a <see cref="RecordViewConfig"/> object.
/// </summary>
internal class WorkflowViewConfig
{
    /// <summary>
    /// Retrieves the workflow view configuration.
    /// </summary>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object that defines the record view layout for workflows,
    /// including titles, buttons, and designated regions.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                            'title': '@ConDefE@',
                            'name': 'workflows',
                            'type': 'recordview',
                            buttons:
                                [
                                    { key: 'stateheader', text: '@ConWorA@', icon: 'ProgressLoopInner', primaryAction: 4, uievent: 'addstateuievent', workflow: true,
                                        buttons: [
                                            { key: 'addstate', text: '@ConAddS@', icon: 'Add', uievent: 'addstateuievent'},
                                            { key: 'editstate', text: '@ConEdiS@', icon: 'Edit', uievent: 'editstateuievent'},
                                            { key: 'deletestate', text: '@ConDelU@', icon: 'Edit', uievent: 'deletestateuievent'}
                                        ]
                                    }
                                ],
                            'regions': [
                                { 'id': 'workflowsteps',
                                   'type': 'listview',
                                   'title': '@ConWor@',
                                   'listViewName': 'workflowstep'
                                }
                            ]
                         }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);
        return result;
    }
}
