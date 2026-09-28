using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddLaboratoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addlaboratoryform',
                        title: 'Add a new laboratory.',
                        saveEvent: 'addlaboratory',
                        suppressRecordView: true,
                        configurable: 'Yes',
                        pages: ['addlaboratorypage']
                    }";

            return form;
        }
    }
}
