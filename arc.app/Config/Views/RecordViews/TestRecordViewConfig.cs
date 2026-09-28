using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the Test record view.
/// Displays test contents (direct or culture) in a crafted section when View is clicked from DirectTestsGrid.
/// Includes an Edit top-bar action that opens the same form as list/grid Edit (UI event resolved from TestName at runtime).
/// </summary>
internal class TestRecordViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the Test record view.
    /// </summary>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object that defines the record view layout for tests,
    /// with a crafted region for test contents, instrument results, and an Edit button that refreshes after save.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@GenVieB@',
                        'name': 'testrecordview',
                        'type': 'recordview',
                        'buttons': [
                            { 'key': 'edittest', 'text': '@GenEdi@', 'icon': 'Edit', 'onSelect': false, 'primaryAction': 1, 'workflow': false, 'onFinish': 'refresh' }
                        ],
                        'regions': [
                            { 'id': 'testcontents', 'type': 'crafted', 'title': '', 'name': 'testrecordviewsection', 'queryName': 'testrecordviewbyid' },
                            { 'id': 'testinstrumentresults', 'type': 'listview', 'title': '@InsIns@', 'listViewName': 'testinstresults' }
                        ]
                    }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}
