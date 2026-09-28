using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteReportConfigFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletereportconfigform',
                        viewTitle: 'Delete report config.',
                        saveEvent: 'deletereportconfig',
                        suppressRecordView: true,
                        initialQuery: 'deletereportconfigquery',
                        pages: [ 'deletereportconfigpage']
                    }";

            return form;
        }
    }
}
