using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the "Report List Config" view.
/// </summary>
internal class ReportListConfigViewConfig
{
    /// <summary>
    /// Retrieves the configuration for the "Report List Config" view.
    /// </summary>
    /// <remarks>
    /// This configuration defines the layout and structure of the "Report List Config" record view. 
    /// It includes metadata such as title, name, type, and regions. Regions represent specific sections 
    /// of the view, each tied to particular data queries or crafted configurations.
    /// </remarks>
    /// <returns>
    /// A <see cref="RecordViewConfig"/> object representing the record view configuration, including
    /// details about regions and their associated query names.
    /// </returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                            'title': '@ConDefB@',
                            'name': 'reportlistconfig',
                            'type': 'recordview',
                            buttons: [
                            ],
                            'regions': [
                                {
                                  'id': 'reportdefinition',
                                  'type': 'crafted',
                                  'title': '',
                                  'name': 'reportdefinition',
                                  'queryName': 'reportdefinitionquery'
                                }
                            ]
                         }";

        var result = JsonConvert.DeserializeObject<RecordViewConfig>(view);

        return result;
    }
}
