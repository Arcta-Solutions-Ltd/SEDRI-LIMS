using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory class for creating UI event configuration instances based on a definition name.
/// </summary>
internal class GraphUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates an <see cref="IDefinition"/> instance corresponding to the specified UI event name.
    /// </summary>
    /// <param name="definitionName">The name of the UI event definition to instantiate.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> implementation matching the given name, or <c>null</c> if no match is found.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "gendersummaryuievent" => new GenderSummaryUIEventConfig(),
            "locationgraphuievent" => new LocationGraphUIEventConfig(),
            "organisationgraphuievent" => new OrganisationGraphUIEventConfig(),
            "organismgraphuievent" => new OrganismGraphUIEventConfig(),
            "organismsusceptibilitygraphuievent" => new OrganismSusceptibilityGraphUIEventConfig(),
            "specimentypesummaryuievent" => new SpecimenTypeSummaryUIEventConfig(),
            "specimenstategraphuievent" => new SpecimenStateGraphUIEventConfig(),
            "taggraphuievent" => new TagGraphUIEventConfig(),
            "testgraphuievent" => new TestGraphUIEventConfig(),
            _ => null,
        };
    }
}
