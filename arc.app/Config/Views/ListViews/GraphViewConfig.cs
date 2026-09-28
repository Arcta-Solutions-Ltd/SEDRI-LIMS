using arc.domain.Configuration.ViewConfig.ListViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.ListViews;

/// <summary>
/// Defines the configuration for the graph view used in the UI.
/// </summary>
internal class GraphViewConfig
{
    /// <summary>
    /// Returns a deserialized <see cref="ListViewConfig"/> object representing the graph view layout.
    /// </summary>
    /// <returns>
    /// A <see cref="ListViewConfig"/> instance containing metadata, title, header text, and button definitions
    /// for managing specimen-related graphs and export actions.
    /// </returns>
    internal ListViewConfig GetView()
    {
        var view = @"{
                            'name': 'graphs',
                            'type': 'ManageList',
                            'title': '@GenGra@',
                            'headerText': '@GraInf@.',
                            'buttons': [
                                { key: 'specimengraphs', text: '@GenGraA@', icon: 'TestBeaker',
                                    buttons: [
                                                { key: 'location', text: '@GraSpeG@', icon: 'PieSingle', uievent: 'locationgraphuievent' },
                                                { key: 'organisation', text: '@GraSpeF@', icon: 'PieSingle', uievent: 'organisationgraphuievent', dynamic: true },
                                                { key: 'organism', text: '@GraSpeH@', icon: 'PieSingle', uievent: 'organismgraphuievent' },
                                                { key: 'organismsus', text: '@GraOrg@', icon: 'PieSingle', uievent: 'organismsusceptibilitygraphuievent' },
                                                { key: 'specimentype', text: '@GraSpe@', icon: 'PieSingle', uievent: 'specimentypesummaryuievent' },
                                                { key: 'specimenstate', text: '@SpeStaA@', icon: 'PieSingle', uievent: 'specimenstategraphuievent' },
                                                { key: 'gender', text: '@GraGen@', icon: 'PieSingle', uievent: 'gendersummaryuievent' },
                                                { key: 'tag', text: '@GraSpeE@', icon: 'PieSingle', uievent: 'taggraphuievent' },
                                                { key: 'test', text: '@GenTesE@', icon: 'PieSingle', uievent: 'testgraphuievent' }
                                        ]
                                },
                                { key: 'exportdata', text: '@GraExp@', icon: 'Installation', uievent: 'locationgraphuievent' },
                                { key: 'exportgraphimage', text: '@GraExpA@', icon: 'Installation', uievent: 'organisationgraphuievent' }
                            ]
                        }";

        var result = JsonConvert.DeserializeObject<ListViewConfig>(view);

        return result;
    }
}
