using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the request record view shown from patient or admission contexts.
/// </summary>
internal class RequestRecordViewConfig
{
    /// <summary>
    /// Retrieves the request record view configuration.
    /// </summary>
    /// <returns>The record view configuration.</returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        title: '@NeoReqRec@',
                        name: 'requestrecordview',
                        type: 'recordview',
                        singleItemName: '@NeoReq@',
                        buttons: [
                            { key: 'edit', text: '@NeoReqEdi@', icon: 'Edit', uievent: 'editrequestuievent', onFinish: 'refresh' },
                            { key: 'manageattachments', text: '@GenAtts@', icon: 'Attach', uievent: 'managerequestattachmentsuievent', onFinish: 'refresh' }
                        ],
                        regions: [
                            { id: 'requestdetails', type: 'standard', queryName: 'requestforrequestview' },
                            { id: 'attachments', type: 'standard', queryName: 'requestattachmentsforrequestview', title: '@GenAtts@' },
                            { id: 'requestspecimens', type: 'listview', title: '@GenSpe@', listViewName: 'requestspecimenlist' },
                            { id: 'requestreports', type: 'crafted', title: '@RepRep@', Name: 'reporthistorygrid', queryName: 'requestreportlist' }
                        ]
                    }";

        return JsonConvert.DeserializeObject<RecordViewConfig>(view);
    }
}
