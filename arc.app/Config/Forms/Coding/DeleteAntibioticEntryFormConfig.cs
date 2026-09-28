using arc.app.Common;

namespace arc.app.Config.Forms.Coding
{
    internal class DeleteAntibioticEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'deleteantibioticentryform',
                    title: '@AntDelB@',
                    viewTitle: 'Delete an antibiotic from the selected list.',
                    saveEvent: 'deleteantibioticentry',
                    initialQuery: 'antibioticentrybyidquery',
                    suppressRecordView: true,
                    pages: ['deleteantibioticentrypage']
                }";

            return form;
        }
    }
}
