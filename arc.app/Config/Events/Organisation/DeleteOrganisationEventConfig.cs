using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class DeleteOrganisationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'deleteOrganisation', 
                        Description: '@OrgDelB@',
                        EventType : 'deletedata', 
                        Topic : 'Organisation', 
                        TableName: 'Organisation',
                        DataRules: [
                            { type: 'NoRecord', query: 'organisationusercount', message: '@OrgThi@' },
                            { type: 'NoRecord', query: 'organisationspecimencount', message: '@OrgThiA@' },
                            { type: 'NoRecord', query: 'organisationchildcount', message: '@OrgThiB@' }
                        ]
                    }";
        }
    }
}
