using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class IqcTestViewMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'iqctestviewmapper', 
                        'Type': 'standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'createddate', Value: 'createddate' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'completeddate', Value: 'completeddate' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'state', Value: 'state' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'method', Value: 'method' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'profilename', Value: 'profilename' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'stateid', Value: 'stateid' }
                        ],
                        'Target': 
                            {
                                Sections: [
                                    {
                                        Id: 'iqctestdetails',
                                        Fields: [
                                            { Id: 'createddate', Label: '@GenDatB@', Value: '<:1:>' },
                                            { Id: 'completeddate', Label: '@GenDatA@', Value: '<:2:>' },
                                            { Id: 'state', Label: '@GenStaA@', Value: '<:3:>' },
                                            { Id: 'method', Label: '@GenMet@', Value: '<:4:>' },
                                            { Id: 'profilename', Label: '@QuaIqcTesProNam@', Value: '<:5:>' }
                                        ]
                                    }
                                ],
                                stateid: '<:6:>'
                            }
                     }";
        }
    }
}
