using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddOrganismEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addOrganism', 
                        Description: '@OrgAddA@',
                        EventType : 'adddata', 
                        Topic : 'Organism', 
                        TableName: 'OrganismCoding',
                        Mapping: 'organismcodingeventmapper',
                        ValidationRules: [
                            { field: 'OrganismId', rule: 'required', message: '@OrgB@'},
                            { field: 'Code', rule: 'required', message: '@CodAC@'}
                        ],
                        DataRules: [
                            { type: 'FindRecord', query: 'organismexists', message: '@CodThe@' },
                            { type: 'NoRecord', query: 'organismcodingexists', message: '@CodTheA@' }
                        ]
                    }";
        }
    }
}
