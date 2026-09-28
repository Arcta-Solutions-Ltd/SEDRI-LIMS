using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddOrganisationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addOrganisation', 
                        Description: '@OrgAdd@',
                        EventType : 'specialadddata',
                        Topic : 'Organisation', 
                        TableName: 'Organisation',
                        ValidationRules: [
                            { field: 'OrganisationName', rule: 'required', message: '@OrgOrgA@'},
                            { field: 'LanguageId', rule: 'required', message: '@LabA@'},
                            { field: 'Code', rule: 'required', message: '@OrgEntC@'}
                        ],
                        DataRules: [
                        { type: 'NoRecord', query: 'organisationforvalidation', message: '@OrgOrgB@' }
                        ]
                    }";
        }
    }
}
