using arc.app.Common;

namespace arc.app.Config.Forms.Config;
internal class DeleteMappingFormConfig : IDefinition
{
    public string Get()
    {
        var form = @"{
                        name: 'deletemappingform',
                        viewTitle: 'Delete mapping.',
                        saveEvent: 'deletemappingevent',
                        suppressRecordView: true,
                        initialQuery: 'deletemappingquery',
                        pages: [ 'deletemappingpage']
                    }";

        return form;
    }
}
