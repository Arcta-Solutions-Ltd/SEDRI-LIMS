using arc.app.Common;

namespace arc.app.Config.Forms.Coding
{
    internal class AddAntibioticEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'addantibioticentryform',
                    title: '@AntAddD@',
                    viewTitle: 'Add an antibiotic to the selected list.',
                    saveEvent: 'addantibioticentry',
                    suppressRecordView: true,
                    pages: ['addantibioticentrypage']
                }";

            return form;
        }
    }
}
