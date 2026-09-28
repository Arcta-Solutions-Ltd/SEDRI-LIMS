using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OrganisationDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'organisationdetailspage',
                            pageTitle: '@OrgAdd@',
                            text: '@OrgEnt@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'OrganisationName', type: 'singleline', label: '@OrgOrg@', required: true, placeholder: '@OrgEntA@', Max: 50 },
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', required: true, placeholder: '@OrgEntB@', Max: 15 },
                                                { id: 'ParentOrganisationId', type: 'picker', label: '@OrgPar@', required: false, placeholder: '@SeaEnt@', optionsName: 'OrganisationList', dynamic: true },
                                                { id: 'LanguageId', type: 'dropdown', label: '@LanSelA@', required: true, placeholder: '@LanSel@', optionsName: 'Translation', dynamic: true },
                                                { id: 'AddressLine1', type: 'singleline', label: '@PatAddD@' },
                                                { id: 'AddressLine2', type: 'singleline', label: '@PatAddE@' },
                                                { id: 'LocationId', type: 'picker', label: '@GenLoc@', placeholder: '@SeaEnt@', optionsName: 'LocationList', dynamic: true },
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes' }
                                           ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
