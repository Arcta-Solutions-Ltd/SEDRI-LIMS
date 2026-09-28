using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class BatchPrintFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'batchprintform',
                        viewTitle: 'Batch Print.',
                        saveEvent: 'specimenreport',
                        suppressRecordView: true,
                        pages: ['batchprintpage']
                    }";

            return form;
        }
    }
}
