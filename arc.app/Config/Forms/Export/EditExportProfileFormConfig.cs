using arc.app.Common;

namespace arc.app.Config.Forms.Export
{
    internal class EditExportProfileFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editexportprofileform',
                        viewTitle: 'Edit a export profile.',
                        title: 'Edit export profile',
                        saveEvent: 'editexportprofile',
                        initialQuery: 'editexportprofile',
                        suppressRecordView: true,
                        pages: ['editexportprofilepage']
                    }";

            return form;
        }
    }
}
