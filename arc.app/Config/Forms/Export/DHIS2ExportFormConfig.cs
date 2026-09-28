using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DHIS2ExportFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'dhis2exportform',
                        viewTitle: 'DHIS2 export',
                        saveEvent: 'dhis2export',
                        suppressRecordView: true,
                        pages: ['dhis2exportpage']
                    }";

            return form;
        }
    }
}
