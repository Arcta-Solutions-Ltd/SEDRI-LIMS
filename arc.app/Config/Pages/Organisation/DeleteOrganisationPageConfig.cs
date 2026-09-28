using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteOrganisationPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteorganisationpage',
                            pageTitle: '@OrgDelC@',
                            text: '@OrgDelD@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'OrganisationName', type: 'text', label: '@GenNam@'}
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}
