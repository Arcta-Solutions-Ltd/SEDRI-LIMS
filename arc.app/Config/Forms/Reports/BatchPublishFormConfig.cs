using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class BatchPublishFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'batchpublishform',
                        viewTitle: 'Batch Publish.',
                        saveEvent: 'specimenreport',
                        suppressRecordView: true,
                        pages: ['batchpublishpage']
                    }";

            return form;
        }
    }
}
