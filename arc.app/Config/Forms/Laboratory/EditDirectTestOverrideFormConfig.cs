using arc.app.Common;

namespace arc.app.Config.Forms;

internal class EditDirectTestOverrideFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'editdirecttestoverrideform',
            viewTitle: '@GenTATD@',
            saveEvent: 'editdirecttestoverride',
            saveOperation: 'updategrid',
            suppressRecordView: true,
            pages: ['adddirecttestoverridepage']
        }";
    }
}
