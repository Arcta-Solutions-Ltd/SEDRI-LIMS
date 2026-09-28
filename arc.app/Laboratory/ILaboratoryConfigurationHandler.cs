using arc.common.Models.Laboratory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Defines methods for handling and loading laboratory configurations.
/// </summary>
public interface ILaboratoryConfigurationHandler
{
    /// <summary>
    /// Gets the list of loaded laboratory configuration models.
    /// </summary>
    LaboratoryConfigurationListModel LaboratoryConfigurationList { get; }

    /// <summary>
    /// Asynchronously loads configuration data for a single laboratory.
    /// </summary>
    /// <param name="laboratoryId">The unique identifier of the laboratory.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task LoadSingleLaboratoryConfigurationAsync(int laboratoryId);

    /// <summary>
    /// Asynchronously loads configuration data for all laboratories.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task LoadConfigurationForAllLaboratoriesAsync();

    /// <summary>
    /// Asynchronously loads the configuration for a given specimen.
    /// </summary>
    /// <param name="specimenId">The unique identifier of the specimen.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task LoadConfigurationForSpecimenAsync(int specimenId);

    /// <summary>
    /// Returns the list of isolate test form names applicable for the given culture type and organism.
    /// Filters by culture type first, then by organism scope when an organism is set.
    /// If the laboratory has a non-empty <c>ResistanceMechanismIsolateTestNames</c> list, intersects with that whitelist.
    /// </summary>
    /// <param name="laboratoryId">The laboratory ID.</param>
    /// <param name="cultureTypeId">The culture type ID.</param>
    /// <param name="organismId">The organism ID (0 if not set).</param>
    /// <param name="orgGroupCodingId">The organism group coding ID (0 if using specific organism).</param>
    /// <returns>List of applicable isolate test form names, or null when no culture type config exists (frontend uses default).</returns>
    Task<IReadOnlyList<string>> GetApplicableIsolateTestsForCultureAsync(int laboratoryId, int cultureTypeId, int organismId, int orgGroupCodingId);

    /// <summary>
    /// Returns the list of isolate test form names selectable on the Select Isolate Tests form for the
    /// given culture type and organism. Applies the culture type and organism scope configuration only.
    /// The laboratory <c>ResistanceMechanismIsolateTestNames</c> whitelist is deliberately not applied
    /// here because it restricts the AST screen alone.
    /// </summary>
    /// <param name="laboratoryId">The laboratory ID.</param>
    /// <param name="cultureTypeId">The culture type ID.</param>
    /// <param name="organismId">The organism ID (0 if not set).</param>
    /// <param name="orgGroupCodingId">The organism group coding ID (0 if using specific organism).</param>
    /// <returns>List of applicable isolate test form names, or null when no configuration applies and every isolate test should be selectable.</returns>
    Task<IReadOnlyList<string>> GetApplicableIsolateTestsForSelectionAsync(int laboratoryId, int cultureTypeId, int organismId, int orgGroupCodingId);
}
