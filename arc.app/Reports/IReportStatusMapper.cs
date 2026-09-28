using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Maps a raw specimen state value to a display-ready string for use in printed reports,
/// optionally prefixed with a colour tag that the PDF renderer understands.
/// </summary>
public interface IReportStatusMapper
{
    /// <summary>
    /// Resolves the configured display text and colour for the given specimen state.
    /// The numeric code for <paramref name="reportStatus"/> is looked up in list 60,
    /// then matched against the <c>reportstatus</c> configuration <c>MappingValues</c>.
    /// When a match is found the return value is formatted as
    /// <c>&lt;:C:{Colour}:&gt;{Text}</c> (e.g. <c>&lt;:C:red:&gt;Preliminary Report</c>),
    /// which the PDF renderer strips into a coloured text run.
    /// When no mapping exists the original value is returned unchanged.
    /// </summary>
    /// <param name="reportStatus">
    /// The raw specimen state string as stored on the specimen record
    /// (e.g. <c>"Pending Approval Level 1"</c>).
    /// </param>
    /// <returns>
    /// A display string for the report heading: either a colour-tagged mapped value
    /// or the original <paramref name="reportStatus"/> when no mapping is configured.
    /// </returns>
    Task<string> AddMappingAsync(string reportStatus);
}
