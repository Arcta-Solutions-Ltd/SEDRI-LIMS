using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class DeleteOrganismMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'deleteorganismmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'OrganismCodingId', Value: 'OrganismCodingId' }
                        ],
                        'Target' : { 
                            Id: '<:1:>'
                        }
                     }";
        }
    }
}
