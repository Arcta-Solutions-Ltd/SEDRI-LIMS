namespace arc.common.Constants;

/// <summary>
/// Stable sidebar keys used for role menu permissions.
/// </summary>
public static class MenuPermissionKeys
{
    /// <summary>
    /// Sidebar key for the Home menu item.
    /// </summary>
    public const string Home = "home";

    /// <summary>
    /// Sidebar keys that must always be granted; matched by stable key, never translated label.
    /// </summary>
    public static readonly string[] MandatorySidebarItems = { Home };
}
