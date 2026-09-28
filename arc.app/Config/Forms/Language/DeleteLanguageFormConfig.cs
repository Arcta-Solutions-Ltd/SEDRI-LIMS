using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteLanguageFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletelanguageform',
                        viewTitle: 'Delete a language.',
                        saveEvent: 'deletelanguage',
                        suppressRecordView: true,
                        finishButtonText: '@GenDelC@',
                        pages: ['deletelanguagepage']
                    }";

            return form;
        }
    }
}
