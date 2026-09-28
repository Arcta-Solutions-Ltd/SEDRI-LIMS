using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Factory class for creating laboratory mapper configurations.
/// </summary>
internal class LaboratoryMapperFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a specific mapper configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the mapper configuration to create.</param>
    /// <returns>
    /// An <see cref="IDefinition"/> object representing the desired mapper configuration, 
    /// or <c>null</c> if the definition name does not match any known configurations.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "laboratoryspecimencountmapper" => new LaboratorySpecimenCountMapper(),
            "laboratoryusercountmapper" => new LaboratoryUserCountMapper(),
            "laboratoryviewmapper" => new LaboratoryViewMapper(),
            _ => null,
        };
    }
}
