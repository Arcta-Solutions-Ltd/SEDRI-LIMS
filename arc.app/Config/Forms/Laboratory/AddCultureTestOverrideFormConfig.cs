using arc.app.Common;

namespace arc.app.Config.Forms;

internal class AddCultureTestOverrideFormConfig : IDefinition
{
    public string Get()
    {
        return @"{
            name: 'addculturetestoverrideform',
            viewTitle: '@GenTATI@',
            saveEvent: 'addculturetestoverride',
            saveOperation: 'updategrid',
            suppressRecordView: true,
            pages: ['addculturetestoverridepage']
        }";
    }
}
