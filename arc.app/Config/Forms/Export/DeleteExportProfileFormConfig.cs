using arc.app.Common;

namespace arc.app.Config.Forms.Export
{
    internal class DeleteExportProfileFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteexportprofileform',
                        viewTitle: 'Delete a export profile',
                        saveEvent: 'deleteexportprofile',
                        initialQuery: 'exportprofilebyid',
                        suppressRecordView: true,
                        pages: ['deleteexportprofilepage']
                    }";

            return form;
        }
    }
}
