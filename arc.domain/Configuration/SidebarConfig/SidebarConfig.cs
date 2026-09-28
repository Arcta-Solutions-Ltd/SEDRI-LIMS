using arc.common.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.Configuration.SidebarConfig;

/// <summary>
/// Represents a sidebar navigation configuration with support for filtering and collapsing link hierarchies.
/// </summary>
/// <remarks>
/// After filtering, any parent with exactly one remaining child is collapsed so the child replaces the parent at the top level.
/// </remarks>
/// <seealso cref="LinkConfig"/>
public class SidebarConfig
{
    /// <summary>
    /// Gets or sets the collection of top-level sidebar links.
    /// </summary>
    /// <value>A list of <see cref="LinkConfig"/> items that define the sidebar.</value>
    public List<LinkConfig> Links { get; set; }

    /// <summary>
    /// Filters the top-level and child links so that only items whose keys exist in
    /// <paramref name="sidebarItemsToKeep"/> remain. If a link retains exactly one child after filtering,
    /// the parent is replaced by that child in the top-level list.
    /// </summary>
    /// <param name="sidebarItemsToKeep">The set of keys that should be retained in the sidebar. Null is treated as an empty set, removing all gated links.</param>
    public void OnlyKeepTheseSidebarItems(string[] sidebarItemsToKeep)
    {
        var keysToKeep = sidebarItemsToKeep.OrEmpty();

        Links.RemoveAll(link => !keysToKeep.Contains(link.Key));

        for (int i = 0; i < Links.Count; i++)
        {
            var link = Links[i];

            if (link.Links != null)
            {
                link.Links.RemoveAll(innerLink => !keysToKeep.Contains(innerLink.Key));

                if (link.Links.Count == 1)
                {
                    Links[i] = link.Links[0];
                }
            }
        }
    }

    /// <summary>
    /// Retrieves a flat list of all view links, consisting of top-level items without children
    /// and the immediate children of items that have them.
    /// </summary>
    /// <returns>A combined list of leaf-level and immediate child <see cref="LinkConfig"/> items.</returns>
    public List<LinkConfig> GetListOfViews()
    {

        var returnConfig = Links.Where(s => s.Links == null).ToList();

        foreach (var link in Links)
        {
            if (link.Links != null && link.Links.Count() > 0)
            {
                returnConfig.AddRange(link.Links);
            }
        }

        return returnConfig;
    }
}
