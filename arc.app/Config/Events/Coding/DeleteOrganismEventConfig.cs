using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteOrganismEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteOrganism',
                        Description: '@OrgDelA@',
                        EventType : 'special',
                        Mapping: 'deleteorganismmapper',
                        Topic : 'Organism',
                        TableName: 'OrganismCoding',
                        DataRules: [
                            { type: 'NoRecord', query: 'organismculturecount', message: '@OrgThiC@' }
                        ]
                    }";
        }
    }
}
