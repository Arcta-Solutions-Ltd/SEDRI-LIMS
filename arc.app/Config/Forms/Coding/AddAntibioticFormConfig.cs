using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddAntibioticFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'addantibioticform',
                    title: '@AntManAdd@',
                    viewTitle: 'Add a new antibiotic.',
                    saveEvent: 'addantibiotic',
                    suppressRecordView: true,
                    pages: ['addantibioticpage']
                }";

            return form;
        }
    }
}
