using arc.app.Common;

namespace arc.app.Config.Forms.Export
{
    internal class RunExportFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'runexportform',
                        viewTitle: 'Run Export',
                        suppressRecordView: true,
                        saveEvent: 'runexportprofile',
                        pages: ['runexportpage']
                    }";

            return form;
        }
    }
}
