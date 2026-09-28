using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteLaboratoryFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'deletelaboratoryform',
                    title: 'Delete a laboratory',
                    initialQuery: 'laboratorybyid',
                    saveEvent: 'deletelaboratory',
                    suppressRecordView: true,
                    configurable: 'Yes',
                    pages: [ 'deletelaboratorypage']
                }";

            return form;
        }
    }
}
