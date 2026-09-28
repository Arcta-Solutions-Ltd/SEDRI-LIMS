using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditLocationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editlocationform',
                        viewTitle: 'Edit an existing location.',
                        saveEvent: 'editlocation',
                        initialQuery: 'locationbyid',
                        suppressRecordView: true,
                        pages: ['editlocationpage']
                    }";

            return form;
        }
    }
}
