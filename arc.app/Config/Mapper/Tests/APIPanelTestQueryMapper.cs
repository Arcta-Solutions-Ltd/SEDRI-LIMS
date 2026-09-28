using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class APIPanelTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'apipaneltestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'apiidpanel', Value: 'apiidpanel' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'idprofile', Value: 'idprofile' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'percentageid', Value: 'percentageid' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                apiidpanel:'<:2:>',
                                idprofile:'<:3:>',
                                percentageid:'<:4:>',
                                printonreport:'<:5:>'
                            }
                     }";
        }
    }
}
