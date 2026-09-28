using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditOrganisationEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editOrganisation', 
                        Description: '@OrgEdi@',
                        EventType : 'special', 
                        Topic : 'Organisation', 
                        TableName: 'Organisation',
                        ValidationRules: [
                            { field: 'OrganisationName', rule: 'required', message: '@OrgOrgA@'},
                            { field: 'LanguageId', rule: 'required', message: '@LabA@'}
                        ]
                    }";
        }

    }
}
