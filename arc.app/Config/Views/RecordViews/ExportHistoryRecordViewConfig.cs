using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the Export History record view.
/// Displays export criteria and a link to download the exported file.
/// </summary>
internal class ExportHistoryRecordViewConfig
{
    /// <summary>
    /// Gets the record view configuration for export history.
    /// </summary>
    internal RecordViewConfig GetView()
    {
        var view = @"{
            'title': '@ExpExpHis@',
            'name': 'exporthistory',
            'type': 'recordview',
            'singleItemName': '@ExpExpHis@',
            'regions': [
                { 'id': 'exporthistory', 'type': 'standard', 'queryName': 'ExportHistoryRecordView' },
                { 'id': 'attachments', 'type': 'standard', 'queryName': 'exporthistoryattachmentsforrecordview', 'title': '@GenAtts@' }
            ]
        }";

        return JsonConvert.DeserializeObject<RecordViewConfig>(view);
    }
}
