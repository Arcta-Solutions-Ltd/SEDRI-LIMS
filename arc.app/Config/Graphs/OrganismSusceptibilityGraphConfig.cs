using arc.app.Common;

namespace arc.app.Config.Graphs;

/// <summary>
/// Provides configuration for the Organism Susceptibility graph used in antimicrobial resistance reporting.
/// </summary>
internal class OrganismSusceptibilityGraphConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration string for the organism susceptibility graph.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string defining graph metadata, type, title, filter options, and date search behavior.
    /// </returns>
    public string Get()
    {
        var graph = @"{ 
                            Name: 'organismsusceptibilitygraph',
                            Type: 'bar',
                            Stack: false,
                            Title: '@GraOrg@',
                            Filters: [
                                { Key: 'graphtype', FieldName: 'graphtypeid', Placeholder: '@GraGra@', MultiSelect: false, Width: 160, OptionsName: 'GraphType', IncludeFixed: true},
                                { Key: 'dateinterval', FieldName: 'dateinterval', Placeholder: '@GraDat@', MultiSelect: false, Width: 160, OptionsName: 'dateinterval', IncludeFixed: true },
                                { Key: 'stateid', FieldName: 'stateid', Placeholder: '@GenSta@', MultiSelect: true, Width: 160, OptionsName: 'SpecimenWorkflowItems', IncludeFixed: true, dropdownwidth: 250 },
                                { Key: 'organismid', FieldName: 'organismid', Placeholder: '@GenOrgA@', MultiSelect: true, Width: 160, OptionsName: 'specimenorganism', IncludeFixed: true, dropdownwidth: 400 },
                                { Key: 'antibioticid', FieldName: 'antibioticid', Placeholder: '@GenAnt@', MultiSelect: true, Width: 160, OptionsName: 'antibiotic', IncludeFixed: true, dropdownwidth: 300 },
                                { Key: 'susceptibilityid', FieldName: 'susceptibilityid', Placeholder: '@GenSus@', MultiSelect: true, Width: 160, OptionsName: 'testresult', IncludeFixed: true, dropdownwidth: 300 },
                                { Key: 'specimentypeid', FieldName: 'specimentypeid', Placeholder: '@GraSpeB@', MultiSelect: true, Width: 160, OptionsName: 'specimentype', IncludeFixed: true, dropdownwidth: 300 },
                                { Key: 'gender', FieldName: 'genderid', Placeholder: '@PatGenA@', MultiSelect: true, Width: 160, OptionsName: 'gender', IncludeFixed: true},
                                { Key: 'tagid', FieldName: 'tagid', Placeholder: '@GenTagA@', MultiSelect: true, Width: 160, OptionsName: 'Tag', IncludeFixed: true, dropdownwidth: 300, Type: 'hierarchicalpicker' },
                                { key: 'locationid', placeholder: '@GenLoc@', width: 160, optionsName: 'LocationList', MultiSelect: true, fieldName: 'LocationId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false },
                                { key: 'organisationfilterid', placeholder: '@GenOrg@', width: 160, optionsName: 'OrganisationList', MultiSelect: true, fieldName: 'OrganisationfilterId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false }
                            ],
                            DateSearch: 'range',
                        }";

        return graph;
    }
}
