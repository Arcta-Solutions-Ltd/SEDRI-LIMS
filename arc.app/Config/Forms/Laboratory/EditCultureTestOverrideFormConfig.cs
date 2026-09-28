using arc.app.Common;

namespace arc.app.Config.Forms;

internal class EditCultureTestOverrideFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'editculturetestoverrideform',
            viewTitle: '@GenTATD@',
            saveEvent: 'editculturetestoverride',
            saveOperation: 'updategrid',
            suppressRecordView: true,
            pages: ['addculturetestoverridepage']
        }";
    }
}
