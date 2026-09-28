using arc.common.ExtensionMethods;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ReportsConfig.ReportToPrint;
using System.Collections.Generic;
using System.Linq;

namespace arc.domain.ExtensionMethods;

/// <summary>
/// Projects report designer API models onto the arc.domain configuration shapes that are serialised
/// into configs.contents. Going through these projections is what keeps transient designer state
/// (ConfigId and State) out of stored configuration, and what guarantees the stored document matches
/// the shape the renderer reads back.
/// </summary>
/// <remarks>
/// These live in arc.domain rather than arc.common/ExtensionMethods because they return arc.domain
/// types, and arc.common cannot reference arc.domain without a circular project reference. The
/// name helpers that have no domain dependency are in arc.common/ExtensionMethods/ReportDesignerExtensions.cs.
/// </remarks>
public static class ReportDesignerPersistenceExtensions
{
    /// <summary>
    /// Converts a custom format from the designer into the persisted section format configuration.
    /// </summary>
    /// <remarks>
    /// The stored name keeps the casing it arrived with. Normalising it here rewrote a seeded format
    /// such as 'DoubleColumnOne' to 'doublecolumnone' on the first save, and every section whose
    /// Format still read 'DoubleColumnOne' then failed to resolve its format in the designer. The
    /// configs record is keyed by the normalised configname that <c>SaveReportDesignerConfigCommand</c>
    /// derives separately, so nothing depends on this value being lower case.
    /// </remarks>
    /// <param name="model">The designer format model.</param>
    /// <param name="nameOverride">Optional name to store instead of the model's own name, used when a new format had to be renamed to avoid a collision.</param>
    /// <returns>The configuration to serialise into configs.contents, or null when the model is null.</returns>
    public static ReportSectionFormatConfig ToPersistedFormat(this CustomFormatModel model, string nameOverride = null)
    {
        if (model == null)
        {
            return null;
        }

        return new ReportSectionFormatConfig
        {
            Name = (nameOverride ?? model.Name)?.Trim(),
            Type = model.Type,
            Description = model.Description,
            Heading = ToLineConfigs(model.Heading),
            Columns = ToColumnConfigs(model.Columns),
            Grids = ToGridColumnConfigs(model.Grids),
            Images = ToImageConfigs(model.Images)
        };
    }

    /// <summary>
    /// Converts a section definition from the designer into the persisted section configuration.
    /// </summary>
    /// <param name="model">The designer section model.</param>
    /// <param name="nameOverride">Optional name to store instead of the model's own name, used when a new section had to be renamed to avoid a collision.</param>
    /// <returns>The configuration to serialise into configs.contents, or null when the model is null.</returns>
    public static ReportSectionConfig ToPersistedSection(this ReportSectionModel model, string nameOverride = null)
    {
        if (model == null)
        {
            return null;
        }

        return new ReportSectionConfig
        {
            Id = model.Id,
            Name = (nameOverride ?? model.Name).NormalisedConfigName(),
            Description = model.Description,
            HeadingText = model.HeadingText,
            Format = model.Format,
            Type = model.Type,
            DataSection = model.DataSection,
            Dynamic = model.Dynamic,
            Fields = ToSectionFieldConfigs(model.Fields),
            Grids = ToSectionGridConfigs(model.Grids),
            LayoutRows = ToSectionLayoutRowConfigs(model.LayoutRows),
            Lines = ToLineConfigs(model.Lines),
            Images = ToImageConfigs(model.Images)
        };
    }

    private static List<LineConfig> ToLineConfigs(List<ReportLineModel> lines)
    {
        return lines?.Select(line => new LineConfig
        {
            Line = line.Line,
            Left = line.Left,
            Field = line.Field,
            Text = line.Text,
            FontSize = line.FontSize.ClampReportLineFontSize(),
            Bold = line.Bold,
            Calc = line.Calc
        }).ToList() ?? [];
    }

    private static List<ColumnConfig> ToColumnConfigs(List<ReportColumnModel> columns)
    {
        return columns?.Select(column => new ColumnConfig
        {
            Left = column.Left,
            Width = column.Width,
            LabelWidth = column.LabelWidth
        }).ToList() ?? [];
    }

    private static List<GridColumnConfig> ToGridColumnConfigs(List<ReportGridModel> grids)
    {
        return grids?.Select(grid => new GridColumnConfig
        {
            Left = grid.Left,
            Width = grid.Width
        }).ToList() ?? [];
    }

    private static List<ImageConfig> ToImageConfigs(List<ReportImageModel> images)
    {
        return images?.Select(image => new ImageConfig
        {
            X = image.X,
            Y = image.Y,
            Label = image.Label,
            Name = image.Name,
            Value = image.Value,
            Width = image.Width,
            Height = image.Height,
            Format = image.Format
        }).ToList() ?? [];
    }

    private static List<ReportSectionFieldConfig> ToSectionFieldConfigs(List<ReportSectionFieldModel> fields)
    {
        return fields?.Select(field => new ReportSectionFieldConfig
        {
            Label = field.Label,
            Value = field.Value,
            Text = field.Text,
            Column = field.Column,
            Order = field.Order,
            Image = field.Image,
            Width = field.Width,
            Height = field.Height,
            Format = field.Format,
            NoBox = field.NoBox
        }).ToList() ?? [];
    }

    private static List<ReportSectionGridConfig> ToSectionGridConfigs(List<ReportSectionGridModel> grids)
    {
        return grids?.Select(grid => new ReportSectionGridConfig
        {
            Name = grid.Name,
            Head = grid.Head ?? [],
            NoBox = grid.NoBox
        }).ToList() ?? [];
    }

    /// <remarks>
    /// Rows the designer left empty are dropped rather than stored. The editor creates an empty row the
    /// moment a drag starts and removes it again when the area lands elsewhere, so persisting empties would
    /// store a placeholder that renders as a blank gap.
    /// </remarks>
    private static List<ReportSectionLayoutRowConfig> ToSectionLayoutRowConfigs(List<ReportSectionLayoutRowModel> rows)
    {
        return rows?
            .Where(row => row?.Areas != null && row.Areas.Count > 0)
            .Select(row => new ReportSectionLayoutRowConfig
            {
                Areas = row.Areas.Select(area => new ReportSectionLayoutAreaConfig
                {
                    Type = area.Type,
                    Name = area.Name,
                    WidthPercent = area.WidthPercent
                }).ToList()
            }).ToList() ?? [];
    }
}
