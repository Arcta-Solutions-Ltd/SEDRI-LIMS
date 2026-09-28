using arc.domain.Configuration.ViewConfig.RecordViewConfig;
using Newtonsoft.Json;

namespace arc.app.Config.Views.RecordViews;

/// <summary>
/// Configuration for the admission record view shown from patient or list contexts.
/// </summary>
internal class AdmissionRecordViewConfig
{
    /// <summary>
    /// Retrieves the admission record view configuration.
    /// </summary>
    /// <returns>The record view configuration.</returns>
    internal RecordViewConfig GetView()
    {
        var view = @"{
                        title: '@NeoAdmRec@',
                        name: 'admissionrecordview',
                        type: 'recordview',
                        singleItemName: '@NeoAdm@',
                        refreshRecordOnEmbeddedSave: true,
                        buttons: [
                            { key: 'edit', text: '@NeoAdmEdi@', icon: 'Edit', uievent: 'editadmissionuievent', onFinish: 'refresh' },
                            { key: 'manageattachments', text: '@GenAtts@', icon: 'Attach', uievent: 'manageadmissionattachmentsuievent', onFinish: 'refresh' }
                        ],
                        regions: [
                            { id: 'admissiondetails', type: 'standard', queryName: 'admissionforadmissionview' },
                            { id: 'attachments', type: 'standard', queryName: 'admissionattachmentsforadmissionview', title: '@GenAtts@' },
                            { id: 'admissionrequests', type: 'listview', title: '@NeoReq@', listViewName: 'admissionrequestslist' },
                            { id: 'admissionspecimens', type: 'listview', title: '@GenSpe@', listViewName: 'admissionspecimenlist' },
                            { id: 'admissionreports', type: 'crafted', title: '@RepRep@', Name: 'reporthistorygrid', queryName: 'admissionreportlist' }
                        ]
                    }";

        return JsonConvert.DeserializeObject<RecordViewConfig>(view);
    }
}
