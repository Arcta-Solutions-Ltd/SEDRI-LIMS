using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditAliquotFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'editaliquotform',
                    viewTitle: '@SpeAliA@',
                    initialQuery: 'aliquotforeditquery',
                    saveEvent: 'editaliquotevent',
                    pages: ['editaliquotpage'],
                    suppressRecordView: true
                }";

            return form;
        }
    }
}
