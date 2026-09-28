using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddCustomFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addcustomform',
                        viewTitle: 'Add a new entry.',
                        saveEvent: 'addcustomentry',
                        suppressRecordView: true,
                        pages: ['addcustompage']
                    }";

            return form;
        }
    }
}
