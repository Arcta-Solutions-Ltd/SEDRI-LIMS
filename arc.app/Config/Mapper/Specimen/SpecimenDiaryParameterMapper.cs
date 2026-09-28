using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SpecimenDiaryParameterMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimendiaryparametermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Key', Where: 'Id', Value: 'Value' }
                        ],
                        'Target' : { 
                            'Name': 'specimendiaryparametermapper', 
                            'Parameters': [ 
                                {'Key': 'SpecimenId', 'Value': '<:1:>'},
                                {'Key': 'EventStatusId', 'Value': '666'}
                            ] 
                        }
                     }";
        }
    }
}
