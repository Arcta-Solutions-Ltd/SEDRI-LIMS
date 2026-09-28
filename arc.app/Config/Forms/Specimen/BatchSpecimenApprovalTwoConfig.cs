using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class BatchSpecimenApprovalTwoConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                            name: 'batchspecimenapprovaltwoform',
                            title: '@SpeFir@',
                            text: 'Batch level two approval',
                            saveEvent: 'batchspecimenapprovaltwo',
                            recordView: 'specimenrecordview',
                            suppressRecordView: true,
                            pages: ['batchspecimenapprovaltwopage']
                        }";

            return form;
        }
    }
}
