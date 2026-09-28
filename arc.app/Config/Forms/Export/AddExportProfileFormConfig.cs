using arc.app.Common;

namespace arc.app.Config.Forms.Export
{
    internal class AddExportProfileFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addexportprofileform',
                        viewTitle: 'Export Profile',
                        saveEvent: 'addexportprofile',
                        suppressRecordView: true,
                        pages: ['addexportprofilepage']
                    }";

            return form;
        }
    }
}
