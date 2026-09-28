using arc.app.Common;

namespace arc.app.Config.Graphs;

/// <summary>
/// Provides configuration for the Specimen Type Summary Graph.
/// </summary>
internal class SpecimenTypeSummaryGraphConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration string for the specimen type summary graph.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string defining graph metadata, type, title, filters, and date search behavior.
    /// </returns>
    public string Get()
    {
        var graph = @"{ 
                            Name: 'specimentypesummarygraph',
                            Type: 'bar',
                            Stack: false,
                            Title: '@GraSpe@',
                            Filters: [
                                { Key: 'graphtype', FieldName: 'graphtypeid', Placeholder: '@GraGra@', MultiSelect: false, Width: 160, OptionsName: 'GraphType', IncludeFixed: true},
                                { Key: 'dateinterval', FieldName: 'dateinterval', Placeholder: '@GraDat@', MultiSelect: false, Width: 160, OptionsName: 'dateinterval', IncludeFixed: true },
                                { Key: 'stateid', FieldName: 'stateid', Placeholder: '@GenSta@', MultiSelect: true, Width: 160, OptionsName: 'SpecimenWorkflowItems', IncludeFixed: true, dropdownwidth: 250 },
                                { Key: 'organismid', FieldName: 'organismid', Placeholder: '@GenOrgA@', MultiSelect: true, Width: 160, OptionsName: 'specimenorganism', IncludeFixed: true, dropdownwidth: 400 },
                                { Key: 'specimentypeid', FieldName: 'specimentypeid', Placeholder: '@GraSpeB@', MultiSelect: true, Width: 160, OptionsName: 'specimentype', IncludeFixed: true, dropdownwidth: 300 },
                                { Key: 'gender', FieldName: 'genderid', Placeholder: '@PatGenA@', MultiSelect: true, Width: 160, OptionsName: 'gender', IncludeFixed: true},
                                { Key: 'testid', FieldName: 'testid', Placeholder: '@GenTesD@', MultiSelect: true, Width: 160, OptionsName: 'directtestconfiglist', IncludeFixed: true, dropdownwidth: 300 },
                                { Key: 'tagid', FieldName: 'tagid', Placeholder: '@GenTagA@', MultiSelect: true, Width: 160, OptionsName: 'Tag', IncludeFixed: true, dropdownwidth: 300, Type: 'hierarchicalpicker' },
                                { key: 'locationid', placeholder: '@GenLoc@', width: 160, optionsName: 'LocationList', MultiSelect: true, fieldName: 'LocationId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false },
                                { key: 'organisationfilterid', placeholder: '@GenOrg@', width: 160, optionsName: 'OrganisationList', MultiSelect: true, fieldName: 'OrganisationfilterId', type: 'hierarchicalpicker', dropdownwidth: 300, allowAdd: false }
                            ],
                            DateSearch: 'range'
                        }";

        return graph;
    }
}


