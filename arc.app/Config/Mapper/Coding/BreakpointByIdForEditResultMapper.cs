using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class BreakpointByIdForEditResultMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'breakpointbyidforeditresultmapping', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AntibioticName', Value: 'AntibioticName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TestMethod', Value: 'TestMethod' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Host', Value: 'Host' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            Id: '<:4:>',
                            AntibioticName: '<:1:>',
                            TestMethod: '<:2:>',
                            Host: '<:3:>'
                        }
                     }";
        }
    }
}
