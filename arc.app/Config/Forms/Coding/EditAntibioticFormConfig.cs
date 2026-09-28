using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditAntibioticFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editantibioticform',
                        viewTitle: 'Edit an antibiotic.',
                        initialQuery: 'antibioticbyidforeditquery',
                        saveEvent: 'editantibiotic',
                        suppressRecordView: true,
                        pages: ['editantibioticpage']
                    }";

            return form;
        }
    }
}
