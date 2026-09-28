using arc.app.Common;

namespace arc.app.Config.Forms.Export
{
    internal class AddExportProfileFieldFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addexportprofilefieldform',
                        viewTitle: 'Add Field to Profile',
                        saveEvent: 'addexportprofilefield',
                        suppressRecordView: true,
                        pages: ['addexportprofilefieldpage']
                    }";

            return form;
        }
    }
}
