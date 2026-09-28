using arc.app.Common;

namespace arc.app.Config.Graphs;

/// <summary>
/// Provides configuration for the Organisation Summary graph used in reporting dashboards.
/// </summary>
internal class OrganisationGraphConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration string for the organisation-based summary graph.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string defining graph metadata, type, title, filter options, and date search behavior.
    /// </returns>
    public string Get()
    {
        var graph = @"{ 
                            Name: 'organisationgraph',
                            Type: 'bar',
                            Stack: false,
                            Title: '@GraSpeF@',
                            Filters: [
                                { Key: 'graphtype', FieldName: 'graphtypeid', Placeholder: '@GraGra@', MultiSelect: false, Width: 160, OptionsName: 'GraphType', IncludeFixed: true},
                                { Key: 'dateinterval', FieldName: 'dateinterval', Placeholder: '@GraDat@', MultiSelect: false, Width: 160, OptionsName: 'dateinterval', IncludeFixed: true },
                                { Key: 'stateid', FieldName: 'stateid', Placeholder: '@GenSta@', MultiSelect: true, Width: 160, OptionsName: 'SpecimenWorkflowItems', IncludeFixed: true, dropdownwidth: 250 },
                                { Key: 'specimentypeid', FieldName: 'specimentypeid', Placeholder: '@GraSpeB@', MultiSelect: true, Width: 160, OptionsName: 'specimentype', IncludeFixed: true, dropdownwidth: 300 },
                                { Key: 'gender', FieldName: 'genderid', Placeholder: '@PatGenA@', MultiSelect: true, Width: 160, OptionsName: 'gender', IncludeFixed: true},
                                { Key: 'tagid', FieldName: 'tagid', Placeholder: '@GenTagA@', MultiSelect: true, Width: 160, OptionsName: 'Tag', IncludeFixed: true, dropdownwidth: 300, Type: 'hierarchicalpicker' },
                                { Key: 'testid', FieldName: 'testid', Placeholder: '@GenTesD@', MultiSelect: true, Width: 160, OptionsName: 'directtestconfiglist', IncludeFixed: true, dropdownwidth: 300 },
                                { key: 'locationid', placeholder: '@GenLoc@', width: 160, optionsName: 'LocationList', MultiSelect: true, fieldName: 'LocationId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false },
                                { key: 'oragnisationlevel', placeholder: '@GraOrgA@', width: 160, optionsName: 'LocationLevel', MultiSelect: false, fieldName: 'organisationlevelid' },
                                { key: 'organisationfilterid', placeholder: '@GenOrg@', width: 160, optionsName: 'OrganisationList', MultiSelect: true, fieldName: 'OrganisationFilterId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false }
                            ],
                            DateSearch: 'range',
                        }";

        return graph;
    }
}
