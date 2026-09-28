using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class RejectSpecimenMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'rejectspecimenconditionmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'SelectReasonId', Value: 'SelectReasonId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'RejectionReason', Value: 'RejectionReason' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'ReceivedCondition', Value: 'ReceivedCondition' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'StateId', Value: 'StateId' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'SelectReasonId': '<:2:>',
                                'RejectionReason':'<:4:>',
                                'ReceivedConditionId':'<:5:>',
                                'StateId':'<:6:>'
                            }
                     }";
        }
    }
}
