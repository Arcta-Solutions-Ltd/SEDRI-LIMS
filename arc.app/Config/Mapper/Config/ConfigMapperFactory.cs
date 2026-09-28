using arc.app.Common;

namespace arc.app.Config.Mapper
{
    /// <summary>
    /// Factory class responsible for creating mapper definitions for configuration-related operations.
    /// This factory implements the Factory pattern to provide a centralized way of creating mapper instances
    /// based on string-based definition names. All mappers created by this factory are related to system
    /// configuration management, particularly state configuration operations.
    /// </summary>
    /// <remarks>
    /// This factory is part of the mapper infrastructure that handles mapping between different layers
    /// of the application (e.g., UI events to data operations). The factory pattern allows for loose
    /// coupling and easy extension when new mapper types need to be added.
    /// </remarks>
    internal class ConfigMapperFactory : IDefinitionFactory
    {
        /// <summary>
        /// Creates a mapper definition instance based on the provided definition name.
        /// The definition name is case-insensitive and matched against known mapper types using a switch expression.
        /// </summary>
        /// <param name="definitionName">The name of the mapper definition to create (case-insensitive). 
        /// Expected values include: "addstatemapper", "editstatemapper", "stateexistsmapper".</param>
        /// <returns>
        /// An IDefinition instance corresponding to the requested mapper, or null if no matching mapper is found.
        /// Returns null for unknown definition names, allowing callers to handle unsupported mapper types gracefully.
        /// </returns>
        /// <example>
        /// <code>
        /// var factory = new ConfigMapperFactory();
        /// var mapper = factory.Create("AddStateMapper"); // Returns AddStateMapper instance
        /// var unknown = factory.Create("UnknownMapper"); // Returns null
        /// </code>
        /// </example>
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                // Mapper for adding new state configurations to the system
                // Used when creating new state entries in the configuration
                "addstatemapper" => new AddStateMapper(),
                
                // Commented out mappers - potentially deprecated or not yet implemented
                // These mappers appear to be related to specimen type deletion operations
                // but are currently disabled, possibly due to refactoring or feature removal
                //"deletespecimentypedirecttestmapper" => new DeleteSpecimenTypeDirectTestMapper(),
                //"deletespecimentypeculturetypemapper" => new DeleteSpecimenTypeCultureTypeMapper(),
                
                // Mapper for editing existing state configurations
                // Used when modifying properties of an existing state entry
                "editstatemapper" => new EditStateMapper(),
                
                // Mapper for checking if a state configuration exists in the system
                // Used for validation purposes to prevent duplicate states or verify state existence
                "stateexistsmapper" => new StateExistsMapper(),
                
                // Default case: return null if no matching mapper is found
                // This allows the calling code to handle unknown mapper types appropriately
                _ => null,
            };
        }
    }
}
