using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ResultExistsInBreakpointMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'resultexistsinbreakpointmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'resultexistsinbreakpointmapper', 
                            'Parameters': [ 
                                {'Key': 'ResultId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}
