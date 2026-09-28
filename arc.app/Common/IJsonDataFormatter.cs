using arc.domain.Configuration.EventsConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Common
{
    /// <summary>
    /// Formats stored JSON result payloads using event display metadata (labels, list lookups, dates).
    /// </summary>
    public interface IJsonDataFormatter
    {
        /// <summary>
        /// Formats JSON using save-event display order (legacy behavior).
        /// </summary>
        /// <param name="contents">JSON string to format (typically test result object or structure).</param>
        /// <param name="eventDetails">Save event configuration with display and translation rules.</param>
        /// <returns>JSON string of formatted display items.</returns>
        Task<string> TranslateAsync(string contents, EventConfig eventDetails);

        /// <summary>
        /// Formats JSON using save-event rules; when <paramref name="preferredRootFieldOrder"/> is provided and non-empty,
        /// top-level fields follow that order first (field ids matching the form layout), then any remaining display entries.
        /// </summary>
        /// <param name="contents">JSON string to format.</param>
        /// <param name="eventDetails">Save event configuration with display and translation rules.</param>
        /// <param name="preferredRootFieldOrder">Field ids in form layout order, or null/empty to use display-only ordering.</param>
        /// <returns>JSON string of formatted display items.</returns>
        Task<string> TranslateAsync(string contents, EventConfig eventDetails, IReadOnlyList<string> preferredRootFieldOrder);
    }
}
