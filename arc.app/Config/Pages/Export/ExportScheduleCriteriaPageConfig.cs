using arc.app.Common;

namespace arc.app.Config.Pages.Export;

/// <summary>
/// Page 1 of the export schedule form: criteria (same as Run Export except no dates).
/// Specimen Type, Specimen State, and Test use multi-select comboboxes matching the Run Export crafted form.
/// </summary>
internal class ExportScheduleCriteriaPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the export schedule criteria page.
    /// </summary>
    public string Get()
    {
        var page = @"{
            name: 'exportschedulecriteriapage',
            pageTitle: '@ExpSchCri@',
            text: '@ExpSchCriD@',
            required: 'Name',
            requiredRule: 'and',
            columns: [
                {
                    key: 'col1',
                    fieldWidth: 'wide',
                    itemWidth: 'wide',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { Id: 'ExportProfileId', Type: 'hidden' },
                                { Id: 'Name', Type: 'singleline', Label: '@GenNam@', Required: true, Max: 100 },
                                { Id: 'OrganismId', Type: 'organismlist', Label: '@GenOrgA@' },
                                { Id: 'SpecimenTypeId', Type: 'combobox', Label: '@SpeSpeB@', OptionsName: 'specimentype', MultiSelect: true, Placeholder: '@SpeSelCA@' },
                                { Id: 'SpecimenStateId', Type: 'combobox', Label: '@GenSta@', OptionsName: 'statelist', MultiSelect: true, Placeholder: '@SpeSelQ@' },
                                { Id: 'TagId', Type: 'hierarchicalpicker', Label: '@GenTagA@', OptionsName: 'tag', MultiSelect: true, Placeholder: '@GenTagE@', SearchPlaceholder: '@GenTagC@' },
                                { Id: 'OrganisationId', Type: 'hierarchicalpicker', Label: '@GenOrg@', OptionsName: 'organisationlist', Dynamic: true, MultiSelect: true, Placeholder: '@ExpSelOrg@', SearchPlaceholder: '@ExpFilOrg@', allowAdd: true },
                                { Id: 'LocationId', Type: 'hierarchicalpicker', Label: '@PatPatJ@', OptionsName: 'locationlist', MultiSelect: true, Placeholder: '@ExpSelLoc@', SearchPlaceholder: '@ExpFilLoc@', allowAdd: true },
                                { Id: 'TestId', Type: 'combobox', Label: '@GenTes@', OptionsName: 'directtestconfiglist', MultiSelect: true, Placeholder: '@CulSelA@' },
                                { Id: 'ASTExclusive', Type: 'toggle', Label: '@ExpASTExc@' }
                            ]
                        }
                    ]
                }
            ]
        }";

        return page;
    }
}
