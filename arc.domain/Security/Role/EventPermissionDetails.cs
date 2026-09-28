using arc.common.ExtensionMethods;
using System.Linq;

namespace arc.domain.Security.Role;

/// <summary>
/// Represents the set of events a user is permitted to invoke.
/// </summary>
public class EventPermissionDetails
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventPermissionDetails"/> class
    /// with no allowed events.
    /// </summary>
    public EventPermissionDetails()
    {
        AllowedEvents = new string[] { };
    }

    /// <summary>
    /// Gets or sets the list of event names that are permitted.
    /// </summary>
    public string[] AllowedEvents { get; set; }

    /// <summary>
    /// Merges the specified event items into the existing allowed events,
    /// ensuring there are no duplicates.
    /// </summary>
    /// <param name="eventItems">The event names to add.</param>
    internal void CombineEventItems(string[] eventItems)
    {
        AllowedEvents = AllowedEvents.OrEmpty().Union(eventItems.OrEmpty()).ToArray();
    }
}
