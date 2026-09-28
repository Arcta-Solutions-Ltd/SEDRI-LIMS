using arc.app.Common;

namespace arc.app.Config.Forms.Config;
internal class AddMappingFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'addmappingform',
                        viewTitle: 'Add mapping.',
                        saveEvent: 'addmappingevent',
                        suppressRecordView: true,
                        pages: [ 'addmappingpage']
                    }";

        return form;
    }
}
