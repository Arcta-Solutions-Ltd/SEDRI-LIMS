using arc.app.Common;

namespace arc.app.Config.Forms.Coding
{
    internal class DeleteAntibioticFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteantibioticform',
                        viewTitle: 'Delete Antibiotic.',
                        saveEvent: 'deleteantibiotic',
                        initialQuery: 'antibioticbyidforeditquery',
                        suppressRecordView: true,
                        pages: ['deleteantibioticpage']
                    }";

            return form;
        }
    }
}
