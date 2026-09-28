using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class BatchSpecimenApprovalTwoEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'batchspecimenapprovaltwo', 
                        Description: '@SpeBatC@',
                        EventType: 'batch', 
                        Topic : 'Specimen', 
                        TableName: 'Specimen',
                        BatchEvent: 'specimenapprovaltwo'
                    }";
        }
    }
}
