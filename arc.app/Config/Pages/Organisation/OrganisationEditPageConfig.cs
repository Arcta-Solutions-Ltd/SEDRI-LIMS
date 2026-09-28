using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class OrganisationEditPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'organisationeditpage',
                            pageTitle: '@OrgEdi@',
                            text: '@OrgEdiA@.',
                            configureActions: 'edit',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'OrganisationName', type: 'singleline', label: '@OrgOrg@', required: false, placeholder: '@OrgEntA@', Configurable: 'No', Max: 50 },
                                                { id: 'Code', type: 'singleline', label: '@GenCodA@', placeholder: '@OrgEntB@', Max: 15 },
                                                { id: 'ParentOrganisationId', type: 'picker', label: '@OrgPar@', required: false, placeholder: '@SeaEnt@', optionsName: 'OrganisationListUnfiltered', dynamic: true, Configurable: 'No' },
                                                { id: 'LanguageId', type: 'dropdown', label: '@LanSel@', required: true, placeholder: '@LanSel@', optionsName: 'Translation', dynamic: true, Configurable: 'No' },
                                                { id: 'AddressLine1', type: 'singleline', label: '@PatAddD@' },
                                                { id: 'AddressLine2', type: 'singleline', label: '@PatAddE@' },
                                                { id: 'LocationId', type: 'picker', label: '@GenLoc@', placeholder: '@SeaEnt@', optionsName: 'LocationList', dynamic: true, Configurable: 'No' },
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@', required: true, value: 'Yes', defaultValue: 'Yes', Configurable: 'No' }
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
