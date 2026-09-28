using arc.common.ExtensionMethods;
using arc.domain.Security.Role;
using System;

namespace arc.domain.ExtensionMethods;

/// <summary>
/// Normalizes role permission detail objects so null collections are treated as empty grants.
/// </summary>
public static class PermissionDetailsExtensions
{
    /// <summary>
    /// Ensures <see cref="MenuPermissionDetails.AllowedSidebarItems"/> is never null.
    /// </summary>
    /// <param name="details">The menu permission details to normalize, which may be null.</param>
    /// <returns>A non-null <see cref="MenuPermissionDetails"/> instance with a non-null sidebar item array.</returns>
    public static MenuPermissionDetails NormalizeMenu(this MenuPermissionDetails? details)
    {
        if (details == null)
        {
            return new MenuPermissionDetails();
        }

        details.AllowedSidebarItems = details.AllowedSidebarItems.OrEmpty();
        return details;
    }

    /// <summary>
    /// Ensures <see cref="EventPermissionDetails.AllowedEvents"/> is never null.
    /// </summary>
    /// <param name="details">The event permission details to normalize, which may be null.</param>
    /// <returns>A non-null <see cref="EventPermissionDetails"/> instance with a non-null event array.</returns>
    public static EventPermissionDetails NormalizeEvents(this EventPermissionDetails? details)
    {
        if (details == null)
        {
            return new EventPermissionDetails();
        }

        details.AllowedEvents = details.AllowedEvents.OrEmpty();
        return details;
    }
}
