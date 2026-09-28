using arc.app.Common;

namespace arc.app.Config.Pages.Export
{
    internal class RunExportPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'runexportpage',
                            pageTitle: '@RunExpT@',
                            text: '@RunExpD@',
                            crafted: true,
                            required: 'Start,End',
                            requiredRule: 'and',
                            nextButton: { show: false },
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { Id: 'OrganismId', Type: 'organismlist' },
                                                { Id: 'SpecimenTypeId', Type: 'combobox', optionsName:'specimentype' },
                                                { Id: 'SpecimenStateId', Type: 'combobox', optionsName: 'statelist' },
                                                { Id: 'TagId', Type: 'hierarchicalpicker', optionsName:'tag', multiSelect: true, placeholder: '@GenTagE@', searchPlaceholder: '@GenTagC@' },
                                                { Id: 'OrganisationId', Type: 'hierarchicalpicker', optionsName:'organisationlist', dynamic: true, multiSelect: true, placeholder: '@ExpSelOrg@', searchPlaceholder: '@ExpFilOrg@', allowAdd: true },
                                                { Id: 'LocationId', Type: 'hierarchicalpicker', optionsName:'locationlist', multiSelect: true, placeholder: '@ExpSelLoc@', searchPlaceholder: '@ExpFilLoc@', allowAdd: true },
                                                { Id: 'TestId', Type: 'combobox', optionsName:'directtestconfiglist' },
                                                { Id: 'StartDate', Type: 'date' },
                                                { Id: 'EndDate', Type: 'date' },
                                                { Id: 'ASTExclusive', Type: 'toggle' },
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

