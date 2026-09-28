using arc.common;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ReportsConfig.ReportToPrint;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Mapper class to convert ReportSectionFormatConfig to CustomFormatModel.
/// </summary>
public class ReportSectionFormatConfigToCustomFormatModelMapper : IMapType<ReportSectionFormatConfig, CustomFormatModel>
{
    /// <summary>
    /// Maps a ReportSectionFormatConfig to a CustomFormatModel.
    /// </summary>
    /// <param name="source">The ReportSectionFormatConfig to map.</param>
    /// <returns>A CustomFormatModel with mapped data.</returns>
    public CustomFormatModel Map(ReportSectionFormatConfig source)
    {
        return new CustomFormatModel
        {
            Name = source.Name,
            Type = source.Type,
            Description = source.Description,
            Heading = MapHeading(source.Heading),
            Columns = MapColumns(source.Columns),
            Grids = MapGrids(source.Grids),
            Images = MapImages(source.Images)
        };
    }

    /// <summary>
    /// Maps the heading from ReportSectionFormatConfig to ReportLineModel list.
    /// </summary>
    /// <param name="heading">The list of LineConfig to map.</param>
    /// <returns>A list of ReportLineModel objects.</returns>
    private static List<ReportLineModel> MapHeading(List<LineConfig> heading)
    {
        if (heading == null || !heading.Any())
        {
            return new List<ReportLineModel>();
        }

        return heading.Select(h => new ReportLineModel
        {
            Line = h.Line,
            Left = h.Left,
            Field = h.Field,
            Text = h.Text,
            FontSize = h.FontSize,
            Bold = h.Bold,
            Calc = h.Calc
        }).ToList();
    }

    /// <summary>
    /// Maps the columns from ReportSectionFormatConfig to ReportColumnModel list.
    /// Format definitions are layout-only, so Fields are excluded.
    /// </summary>
    /// <param name="columns">The list of ColumnConfig to map.</param>
    /// <returns>A list of ReportColumnModel objects.</returns>
    private static List<ReportColumnModel> MapColumns(List<ColumnConfig> columns)
    {
        if (columns == null || !columns.Any())
        {
            return new List<ReportColumnModel>();
        }

        return columns.Select(c => new ReportColumnModel
        {
            Left = c.Left,
            Width = c.Width,
            LabelWidth = c.LabelWidth
            // Fields are excluded - format definitions are layout-only
        }).ToList();
    }

    /// <summary>
    /// Maps the grids from ReportSectionFormatConfig to ReportGridModel list.
    /// Format definitions are layout-only, so only Left and Width are included.
    /// </summary>
    /// <param name="grids">The list of GridColumnConfig to map.</param>
    /// <returns>A list of ReportGridModel objects.</returns>
    private static List<ReportGridModel> MapGrids(List<GridColumnConfig> grids)
    {
        if (grids == null || !grids.Any())
        {
            return new List<ReportGridModel>();
        }

        return grids.Select(g => new ReportGridModel
        {
            Left = g.Left,
            Width = g.Width
            // LabelWidth and Fields are excluded - format grid definitions only contain layout properties
        }).ToList();
    }

    /// <summary>
    /// Maps the images from ReportSectionFormatConfig to ReportImageModel list.
    /// </summary>
    /// <param name="images">The list of ImageConfig to map.</param>
    /// <returns>A list of ReportImageModel objects.</returns>
    private static List<ReportImageModel> MapImages(List<ImageConfig> images)
    {
        if (images == null || !images.Any())
        {
            return new List<ReportImageModel>();
        }

        return images.Select(i => new ReportImageModel
        {
            X = i.X,
            Y = i.Y,
            Label = i.Label,
            Name = i.Name,
            Value = i.Value,
            Width = i.Width,
            Height = i.Height,
            Format = i.Format
        }).ToList();
    }

    /// <summary>
    /// Maps the fields from FieldConfig to ReportFieldModel list.
    /// </summary>
    /// <param name="fields">The list of FieldConfig to map.</param>
    /// <returns>A list of ReportFieldModel objects.</returns>
    private static List<ReportFieldModel> MapFields(List<FieldConfig> fields)
    {
        if (fields == null || !fields.Any())
        {
            return new List<ReportFieldModel>();
        }

        return fields.Select(f => new ReportFieldModel
        {
            Label = f.Label,
            Value = f.Value,
            Text = f.Text,
            Image = f.Image,
            Width = f.Width,
            Height = f.Height,
            Format = f.Format
        }).ToList();
    }
}
