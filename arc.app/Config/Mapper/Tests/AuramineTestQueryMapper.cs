using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AuramineTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'auraminetestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'auramineid', Value: 'auramineid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                auramineid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}
