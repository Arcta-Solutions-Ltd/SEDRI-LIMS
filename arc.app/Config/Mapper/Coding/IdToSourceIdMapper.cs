using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class IdToSourceIdMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'idtosourceidmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'checkwhethersourcecontainsbreakpoints', 
                            'Parameters': [ 
                                {'Key': 'SourceId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
