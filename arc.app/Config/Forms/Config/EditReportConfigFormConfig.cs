using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditReportConfigFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editreportconfigform',
                        viewTitle: 'Edit report config.',
                        saveEvent: 'editreportconfig',
                        suppressRecordView: true,
                        initialQuery: 'editreportconfigquery',
                        pages: [ 'editreportconfigpage']
                    }";

            return form;
        }
    }
}
