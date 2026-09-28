using arc.app.Common;

namespace arc.app.Config.Forms.Config;
internal class EditMappingFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'editmappingform',
                        viewTitle: 'Edit mapping.',
                        saveEvent: 'editmappingevent',
                        suppressRecordView: true,
                        initialQuery: 'editmappingquery',
                        pages: [ 'editmappingpage']
                    }";

        return form;
    }
}
