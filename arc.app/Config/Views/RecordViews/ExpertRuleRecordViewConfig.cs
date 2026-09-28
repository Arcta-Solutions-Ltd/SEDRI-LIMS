using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the Expert Rule record view.
/// Displays expert rule details at the top when View is clicked from the expert rules list view,
/// with embedded list sections for conditions, test conditions, and actions.
/// </summary>
internal class ExpertRuleRecordViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the Expert Rule record view.
    /// </summary>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object that defines the record view layout for expert rules,
    /// with expert rule details at the top (standard region) and embedded list views for conditions,
    /// test conditions, and actions.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@RulMan@',
                        'name': 'expertrules',
                        'type': 'recordview',
                        'refreshRecordOnEmbeddedSave': true,
                        buttons: [
                            { key: 'editexpertrule', text: '@RulEdiA@', icon: 'Edit', uievent: 'editexpertruleuievent', onFinish: 'refresh' }
                        ],
                        'regions': [
                            { 'id': 'expertruledetails', 'type': 'standard', 'queryName': 'expertruleviewquery' },
                            { 'id': 'expertruleconditions', 'type': 'listview', 'title': '@GenRulI@', 'listViewName': 'expertruleconditions' },
                            { 'id': 'expertruletestconditions', 'type': 'listview', 'title': '@RulTes@', 'listViewName': 'expertruletestconditions' },
                            { 'id': 'expertruleactions', 'type': 'listview', 'title': '@GenRulF@', 'listViewName': 'expertruleactions' },
                            { 'id': 'expertruleapprovals', 'type': 'listview', 'title': '@BreAppHis@', 'listViewName': 'expertruleapprovals' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}
