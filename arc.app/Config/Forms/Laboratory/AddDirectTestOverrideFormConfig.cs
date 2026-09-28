using arc.app.Common;

namespace arc.app.Config.Forms;

internal class AddDirectTestOverrideFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'adddirecttestoverrideform',
            viewTitle: '@GenTATG@',
            saveEvent: 'adddirecttestoverride',
            saveOperation: 'updategrid',
            suppressRecordView: true,
            pages: ['adddirecttestoverridepage']
        }";
    }
}
