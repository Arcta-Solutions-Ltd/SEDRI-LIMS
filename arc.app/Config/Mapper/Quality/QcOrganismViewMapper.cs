using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class QcOrganismViewMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                'Name': 'qcorganismviewmapper', 
                'Type': 'standard',
                'Rules': [
                    { Key: '<:1:>', Type: 'Mapping', Source: 'organism', Value: 'organism' },
                    { Key: '<:2:>', Type: 'Mapping', Source: 'standardsbody', Value: 'standardsbody' },
                    { Key: '<:3:>', Type: 'Mapping', Source: 'enabled', Value: 'enabled' },
                    { Key: '<:4:>', Type: 'Mapping', Source: 'usebydefault', Value: 'usebydefault' },
                    { Key: '<:5:>', Type: 'Mapping', Source: 'primarystrain', Value: 'primarystrain' },
                    { Key: '<:6:>', Type: 'Mapping', Source: 'otherstrains', Value: 'otherstrains' }
                ],
                'Target': 
                    {
                        Sections: [
                            {
                                Id: 'organismdetails',
                                Fields: [
                                    { Id: 'name', Label: '@GenEntB@', Value: '<:1:>' },
                                    { Id: 'standardsbody', Label: '@ManQcStaBod@', Value: '<:2:>' },
                                    { Id: 'primarystrain', Label: '@ManQcPriStr@', Value: '<:5:>' },
                                    { Id: 'otherstrains', Label: '@ManQcOthStr@', Value: '<:6:>' },
                                    { Id: 'enabled', Label: '@GenEna@', Value: '<:3:>' },
                                    { Id: 'usebydefault', Label: '@GenDef@', Value: '<:4:>' }
                                ]
                            }
                        ]
                    }
                }";
        }
    }
}
