using System.Collections.Generic;
using System.Linq;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;

namespace arc.domain.ExtensionMethods;

/// <summary>
/// Normalises a layout section's stored area row arrangement against the section's live bindings.
/// </summary>
/// <remarks>
/// The stored arrangement and the section's grid bindings drift apart whenever a grid is unbound, a format
/// loses grid positions, or a section gains a grid it has never arranged. Resolving the two on every read
/// means the renderer and the designer always see a complete, non-duplicated arrangement covering exactly
/// the areas the section currently has, without needing a migration when bindings change.
/// <para>
/// Areas reference grids by the stable data section grid id, so resolution compares ids through
/// <see cref="ReportGridExtensions.IsSameGridId"/> and never compares descriptions or translated labels.
/// </para>
/// </remarks>
public static class ReportSectionLayoutRowExtensions
{
    /// <summary>
    /// Resolves the arrangement a section should render with.
    /// </summary>
    /// <param name="rows">The arrangement stored on the section, which may be empty, stale or incomplete.</param>
    /// <param name="grids">The section's grid bindings, in the order that maps them to format grid positions.</param>
    /// <param name="hasFieldBlock">Whether the section renders a scalar field block at all.</param>
    /// <returns>
    /// One row per rendered row, covering every current area exactly once. An empty or absent stored
    /// arrangement resolves to the legacy stacked order: the field block, then each grid on its own row.
    /// </returns>
    public static List<ReportSectionLayoutRowConfig> ResolveLayoutRows(
        this List<ReportSectionLayoutRowConfig> rows,
        List<ReportSectionGridConfig> grids,
        bool hasFieldBlock)
    {
        var boundGridIds = (grids ?? [])
            .Where(grid => !string.IsNullOrWhiteSpace(grid?.Name))
            .Select(grid => grid.Name)
            .ToList();

        var resolved = new List<ReportSectionLayoutRowConfig>();
        var placedGridIds = new List<string>();
        var fieldBlockPlaced = false;

        foreach (var row in rows ?? [])
        {
            var areas = new List<ReportSectionLayoutAreaConfig>();

            foreach (var area in row?.Areas ?? [])
            {
                if (LayoutAreaTypes.IsFieldsArea(area?.Type))
                {
                    if (!hasFieldBlock || fieldBlockPlaced)
                    {
                        continue;
                    }

                    fieldBlockPlaced = true;
                    areas.Add(CloneArea(area, LayoutAreaTypes.Fields, null));
                    continue;
                }

                if (!LayoutAreaTypes.IsGridArea(area?.Type))
                {
                    continue;
                }

                var boundGridId = boundGridIds.FirstOrDefault(id => id.IsSameGridId(area.Name));
                if (boundGridId == null || placedGridIds.Any(id => id.IsSameGridId(boundGridId)))
                {
                    continue;
                }

                placedGridIds.Add(boundGridId);
                areas.Add(CloneArea(area, LayoutAreaTypes.Grid, boundGridId));
            }

            if (areas.Count > 0)
            {
                resolved.Add(new ReportSectionLayoutRowConfig { Areas = areas });
            }
        }

        if (hasFieldBlock && !fieldBlockPlaced)
        {
            resolved.Insert(0, new ReportSectionLayoutRowConfig
            {
                Areas = [new ReportSectionLayoutAreaConfig { Type = LayoutAreaTypes.Fields }]
            });
        }

        foreach (var gridId in boundGridIds.Where(id => !placedGridIds.Any(placed => placed.IsSameGridId(id))))
        {
            resolved.Add(new ReportSectionLayoutRowConfig
            {
                Areas = [new ReportSectionLayoutAreaConfig { Type = LayoutAreaTypes.Grid, Name = gridId }]
            });
        }

        return resolved;
    }

    /// <summary>
    /// Determines whether an arrangement actually places two or more areas together on any row.
    /// </summary>
    /// <param name="rows">A resolved arrangement.</param>
    /// <returns>True when at least one row holds more than one area.</returns>
    public static bool HasSideBySideRow(this List<ReportSectionLayoutRowConfig> rows)
    {
        return (rows ?? []).Any(row => (row?.Areas?.Count ?? 0) > 1);
    }

    /// <summary>
    /// Copies an area, replacing its type and grid id with the resolved values.
    /// </summary>
    /// <param name="area">The stored area, which may be null.</param>
    /// <param name="type">The canonical area type to store.</param>
    /// <param name="name">The bound grid id to store, or null for the field block.</param>
    /// <returns>A new area carrying the resolved identity and the stored width share.</returns>
    private static ReportSectionLayoutAreaConfig CloneArea(ReportSectionLayoutAreaConfig area, string type, string name)
    {
        return new ReportSectionLayoutAreaConfig
        {
            Type = type,
            Name = name,
            WidthPercent = area?.WidthPercent ?? 0
        };
    }
}
