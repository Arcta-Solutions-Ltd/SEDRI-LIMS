using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddLocationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addlocationform',
                        viewTitle: 'Add a new location.',
                        saveEvent: 'addlocation',
                        suppressRecordView: true,
                        pages: ['addlocationpage']
                    }";

            return form;
        }
    }
}
