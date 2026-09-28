using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditReportSectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editreportsectionform',
                        viewTitle: 'Edit report section.',
                        saveEvent: 'editreportsection',
                        suppressRecordView: true,
                        initialquery: 'editreportsectionquery',
                        pages: [ 'editreportsectionpage']
                    }";

            return form;
        }
    }
}
