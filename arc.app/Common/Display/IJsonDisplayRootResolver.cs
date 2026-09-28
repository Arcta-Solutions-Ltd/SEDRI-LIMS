using arc.domain.Configuration.EventsConfig;

namespace arc.app.Common.Display
{
    /// <summary>
    /// Narrows a stored event payload down to the object whose properties the event's display rows
    /// describe. Crafted forms nest their whole payload (for example under
    /// <c>Crafted[0].Contents[0].value</c>) so their display rows have nothing to match at the payload
    /// root; <see cref="EventConfig.DisplayRoot"/> tells the pipeline where to start instead.
    /// </summary>
    public interface IJsonDisplayRootResolver
    {
        /// <summary>
        /// Returns the JSON that should be treated as the display root.
        /// </summary>
        /// <param name="message">Raw stored payload.</param>
        /// <param name="eventDetails">Event configuration supplying <see cref="EventConfig.DisplayRoot"/>.</param>
        /// <returns>
        /// The JSON found at the configured root, or <paramref name="message"/> unchanged when no root is
        /// configured, when the payload cannot be parsed, or when the configured path does not resolve.
        /// </returns>
        string Resolve(string message, EventConfig eventDetails);
    }
}
