using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Provides configuration for the Specimen Report view.
/// </summary>
internal class SpecimenReportViewConfig
{
    /// <summary>
    /// Deserializes a hard-coded JSON string into a <see cref="RecordViewConfig"/> object.
    /// The configuration includes metadata, layout regions, and queries for rendering the specimen view.
    /// </summary>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object representing the UI and data bindings for the specimen report.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@SpeSpeG@',
                        'name': 'specimenreportview',
                        'type': 'recordview',
                        'workflow': 'unapprovedreportworkflow',
                        'singleItemName': '@SinSpe@',
                        singleQuery: 'singlespecimenforspecimenlist',
                        buttons: [
                            { key: 'approvereport', text: '@RepAppC@', icon: 'RedEye', onSelect: true, uievent: 'approvereportuievent', onFinish: 'update', workflow: true },
                            { key: 'unapprovereport', text: '@RepUnaB@', icon: 'RedEye', onSelect: true, uievent: 'unapprovereportuievent', onFinish: 'update', workflow: true }
                        ],
                        'regions': [
                            { 'id': 'specimendetails', 'type': 'standard', 'queryName': 'specimenforspecimenview' },
                            { 'id': 'directtests', 'type': 'crafted', 'title': '@SpeDir@', 'name': 'directtestsgrid', 'queryName': 'testlistforspecimenlite' },
                            { 'id': 'cultures', 'type': 'listview', 'title': '@SpeCulB@', 'listViewName': 'cultures', 'name': 'isolatelist' },
                            { 'id': 'specimencomments', 'type': 'listview', 'title': '@GenComE@', 'listViewName': 'specimencomments' },
                            { 'id': 'reports', 'type': 'crafted', 'title': '@RepRep@', 'Name': 'reporthistorygrid', 'queryName': 'reportlist' },
                            { 'id': 'instrumentresults', 'type': 'listview', 'title': '@InsIns@', 'listViewName': 'specimeninstresults' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}
