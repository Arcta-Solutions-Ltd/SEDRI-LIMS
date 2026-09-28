using arc.app.Common;

namespace arc.app.Config.Reports.SectionFormats;

/// <summary>
/// Factory interface for creating section format configurations based on definition names.
/// </summary>
public interface ISectionFormatFactory
{
    /// <summary>
    /// Creates an instance of a section format configuration based on the provided definition name.
    /// </summary>
    /// <param name="definitionName">The name of the section format definition.</param>
    /// <returns>An instance of a section format configuration, or null if the definition name is not recognized.</returns>
    IDefinition Create(string definitionName);
}
