using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the instrument result record view (read-only fields for one instrumentresults row).
/// </summary>
internal class InstrumentResultRecordViewConfig
{
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        'title': '@InsIns@',
                        'name': 'instrumentresultrecordview',
                        'type': 'recordview',
                        'regions': [
                            { 'id': 'instrumentresultdetails', 'type': 'standard', 'queryName': 'instrumentresultrecordviewbyid' },
                            { 'id': 'instrumentresultattachments', 'type': 'standard', 'queryName': 'instrumentresultattachmentsforinstrumentresultview', 'title': '@InsUplFil@' }
                        ]
                    }";

        return JsonConvert.DeserializeObject<RecordViewConfig>(view);
    }
}
