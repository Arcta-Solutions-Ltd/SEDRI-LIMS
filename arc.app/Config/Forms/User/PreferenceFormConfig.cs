using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class PreferenceFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'preferenceform',
                        formtype: 'singlepage',
                        initialquery: 'preference',
                        saveEvent: 'preference',
                        suppressRecordView: true,
                        pages: ['preferencepage']
                    }";

            return form;
        }
    }
}
