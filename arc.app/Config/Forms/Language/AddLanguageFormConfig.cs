using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddLanguageFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addlanguageform',
                        viewTitle: 'Add a new language.',
                        saveEvent: 'addlanguage',
                        suppressRecordView: true,
                        pages: ['addlanguagepage']
                    }";

            return form;
        }
    }
}
