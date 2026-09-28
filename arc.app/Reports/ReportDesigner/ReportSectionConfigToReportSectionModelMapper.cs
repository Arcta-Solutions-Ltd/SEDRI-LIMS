using arc.common;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Mapper class to convert ReportSectionConfig to ReportSectionModel.
/// </summary>
public class ReportSectionConfigToReportSectionModelMapper : IMapType<ReportSectionConfig, ReportSectionModel>
{
    /// <summary>
    /// Maps a ReportSectionConfig to a ReportSectionModel.
    /// </summary>
    /// <param name="source">The ReportSectionConfig to map.</param>
    /// <returns>A ReportSectionModel with mapped data.</returns>
    public ReportSectionModel Map(ReportSectionConfig source)
    {
        var reportSectionModel = new ReportSectionModel
        {
            Id = source.Id,
            Name = source.Name,
            Description = source.Description,
            HeadingText = source.HeadingText,
            Format = source.Format,
            Type = source.Type,
            DataSection = source.DataSection,
            Dynamic = source.Dynamic,
            Fields = MapFields(source.Fields),
            Grids = MapGrids(source.Grids),
            LayoutRows = MapLayoutRows(source.LayoutRows),
            Lines = MapLines(source.Lines),
            Images = new List<ReportImageModel>() // Will be populated when image mapping is available
        };

        return reportSectionModel;
    }

    /// <summary>
    /// Maps the fields from ReportSectionConfig to ReportSectionFieldModel.
    /// </summary>
    /// <param name="fields">The list of ReportSectionFieldConfig to map.</param>
    /// <returns>A list of ReportSectionFieldModel objects.</returns>
    private static List<ReportSectionFieldModel> MapFields(List<ReportSectionFieldConfig> fields)
    {
        if (fields == null || !fields.Any())
        {
            return new List<ReportSectionFieldModel>();
        }

        return fields.Select(field => new ReportSectionFieldModel
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
        }).ToList();
    }

    /// <summary>
    /// Maps the grids from ReportSectionConfig to ReportSectionGridModel.
    /// </summary>
    /// <param name="grids">The list of ReportSectionGridConfig to map.</param>
    /// <returns>A list of ReportSectionGridModel objects.</returns>
    private static List<ReportSectionGridModel> MapGrids(List<ReportSectionGridConfig> grids)
    {
        if (grids == null || !grids.Any())
        {
            return new List<ReportSectionGridModel>();
        }

        return grids.Select(grid => new ReportSectionGridModel
        {
            Name = grid.Name,
            Head = grid.Head ?? new List<string>(),
            NoBox = grid.NoBox
        }).ToList();
    }

    /// <summary>
    /// Maps the area row arrangement from ReportSectionConfig to ReportSectionLayoutRowModel.
    /// </summary>
    /// <param name="rows">The list of ReportSectionLayoutRowConfig to map.</param>
    /// <returns>A list of ReportSectionLayoutRowModel objects, empty when the section has no arrangement stored.</returns>
    private static List<ReportSectionLayoutRowModel> MapLayoutRows(List<ReportSectionLayoutRowConfig> rows)
    {
        if (rows == null || !rows.Any())
        {
            return new List<ReportSectionLayoutRowModel>();
        }

        return rows.Select(row => new ReportSectionLayoutRowModel
        {
            Areas = (row.Areas ?? new List<ReportSectionLayoutAreaConfig>())
                .Select(area => new ReportSectionLayoutAreaModel
                {
                    Type = area.Type,
                    Name = area.Name,
                    WidthPercent = area.WidthPercent
                }).ToList()
        }).ToList();
    }

    /// <summary>
    /// Maps the lines from ReportSectionConfig to ReportLineModel.
    /// </summary>
    /// <param name="lines">The list of LineConfig to map.</param>
    /// <returns>A list of ReportLineModel objects.</returns>
    private static List<ReportLineModel> MapLines(List<LineConfig> lines)
    {
        if (lines == null || !lines.Any())
        {
            return new List<ReportLineModel>();
        }

        return lines.Select(line => new ReportLineModel
        {
            // Map line properties - adjust based on actual ReportLineModel structure
            // This is a placeholder implementation
        }).ToList();
    }
}
