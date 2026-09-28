using System.Threading.Tasks;

namespace arc.app.Laboratory;

/// <summary>
/// Enriches SpecimenList JSON result with TurnAroundTimeColour per row.
/// </summary>
public interface ISpecimenListTatEnricher
{
    /// <summary>
    /// Enriches the SpecimenList JSON result by adding TurnAroundTimeColour to each item.
    /// </summary>
    /// <param name="specimenListJson">Serialized list of specimen rows.</param>
    /// <param name="archiveList">When true (SpecimenArchiveList), TAT end time uses the terminal state's last history timestamp for finalised, rejected, and cancelled specimens.</param>
    Task<string> EnrichAsync(string specimenListJson, bool archiveList = false);
}
