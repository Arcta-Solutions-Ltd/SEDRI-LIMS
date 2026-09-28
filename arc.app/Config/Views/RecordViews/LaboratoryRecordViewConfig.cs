using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the "Laboratory Record View."
/// </summary>
internal class LaboratoryRecordViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Laboratory Record View."
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the "Laboratory Record View," including metadata such as
    /// title, name, type, buttons, and regions. The regions represent different sections of the view, each tied to
    /// specific data queries or list views.
    /// </remarks>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object representing the record view configuration,
    /// including details about regions, list views, and query names.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@LabLabC@',
                        'name': 'laboratories',
                        'type': 'recordview',
                        buttons: [
                            { 'key': 'turnaroundtime', 'text': '@GenTAT@', 'icon': 'Clock', 'uievent': 'turnaroundtimeuievent' }
                        ],
                        'regions': [
                            { 'id': 'laboratorydetails', 'type': 'standard', 'queryName': 'laboratoryforlaboratoryviewquery' },
                            { 'id': 'testcategorisation', 'type': 'listview', 'title': '@LabTesB@', 'listViewName': 'testcategorisationview' },
                            { 'id': 'culturetypecategorisation', 'type': 'listview', 'title': '@LabCul@', 'listViewName': 'culturetypecategorisationview' },
                            { 'id': 'specimentypedirecttestoption', 'type': 'listview', 'title': '@LabSpe@', 'listViewName': 'specimentypedirecttestoptionview' },
                            { 'id': 'specimentypedirecttestmapping', 'type': 'listview', 'title': '@ConSpeA@', 'listViewName': 'specimentypedirecttest' },
                            { 'id': 'specimentypeculturetypeoption', 'type': 'listview', 'title': '@LabSpeA@', 'listViewName': 'specimentypeculturetypeoptionview' },
                            { 'id': 'formspecimentypeoption', 'type': 'listview', 'title': '@ConFormSpe@', 'listViewName': 'formspecimentypeoptionview' },
                            { 'id': 'specimentypeculturetypemapping', 'type': 'listview', 'title': '@ConSpe@', 'listViewName': 'specimentypeculturetype' },
                            { 'id': 'culturetypeculturetestoption', 'type': 'listview', 'title': '@LabCulA@', 'listViewName': 'culturetypeculturetestoptionview' },
                            { 'id': 'organismscopeculturetestoption', 'type': 'listview', 'title': '@LabOrgA@', 'listViewName': 'organismscopeculturetestoptionview' },
                            { 'id': 'culturetypeculturetestmapping', 'type': 'listview', 'title': '@ConSpeB@', 'listViewName': 'culturetypeculturetest' },
                            { 'id': 'specimentypeworkflowmapping', 'type': 'listview', 'title': '@LabWor@', 'listViewName': 'specimentypeworkflowmappingview' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}

