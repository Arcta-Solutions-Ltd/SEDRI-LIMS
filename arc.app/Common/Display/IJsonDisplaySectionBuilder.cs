using arc.common.Models.Monitoring;
using arc.domain.Configuration.EventsConfig;
using System.Collections.Generic;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Turns the raw parsed payload structure into the ordered, filtered tree that is rendered for a user.
    /// Only fields named in the event's <see cref="DisplayConfig"/> rows survive, in the order those rows
    /// declare, and nested collections or objects survive only where the event declares a matching
    /// <see cref="DisplaySectionConfig"/>.
    /// </summary>
    public interface IJsonDisplaySectionBuilder
    {
        /// <summary>
        /// Builds the display tree.
        /// </summary>
        /// <param name="fields">Parsed payload structure, rooted at the event's display root.</param>
        /// <param name="eventDetails">Event configuration supplying the display rows and sections.</param>
        /// <param name="preferredRootFieldOrder">
        /// When supplied, these field ids are placed first (in this order) ahead of the configured display
        /// order. Used by callers that already know the order the user saw the fields in.
        /// </param>
        /// <returns>Ordered root items ready for label translation and value resolution.</returns>
        List<JsonItemModel> Build(List<JsonItemModel> fields, EventConfig eventDetails, IReadOnlyList<string> preferredRootFieldOrder);
    }
}
