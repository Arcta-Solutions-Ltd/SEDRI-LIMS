using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class BatchSubmitConfirmationFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'batchsubmitconfirmationform',
                            title: '@SpeFir@',
                            text: 'Batch submit confirmation',
                            saveEvent: 'batchsubmitconfirmation',
                            recordView: 'specimenrecordview',
                            suppressRecordView: true,
                            pages: ['submitconfirmationpage']
                        }";

            return form;
        }
    }
}
