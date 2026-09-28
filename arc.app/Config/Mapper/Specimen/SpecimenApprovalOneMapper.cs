//using arc.app.Common;

//namespace arc.app.Config.Mapper
//{
//    internal class SpecimenApprovalOneMapper : IDefinition
//    {
//        public string Get()
//        {
//            return @"{  
//                        'Name': 'addapprovalonemapper', 
//                        'Type': 'Standard',
//                        'Rules': [
//                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
//                            { Key: '<:2:>', Type: 'Mapping', Source: 'Decision', Value: 'Decision' },
//                            { Key: '<:3:>', Type: 'Mapping', Source: 'Reason', Value: 'Reason' },
//                            { Key: '<:4:>', Type: 'Mapping', Source: 'ApprovalCommentId', Value: 'ApprovalCommentId' },
//                            { Key: '<:5:>', Type: 'Mapping', Source: 'StateId', Value: 'StateId' }
//                        ],
//                        'Target': 
//                            {
//                                'Id':'<:1:>',
//                                'ReasonOne':'<:3:>',
//                                'ApprovalCommentL1Id':'<:4:>',
//                                'StateId':'<:5:>'
//                            }
//                     }";
//        }
//    }
//}
