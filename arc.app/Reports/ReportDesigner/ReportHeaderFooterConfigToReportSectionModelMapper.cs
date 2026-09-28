using arc.common;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Reports.ReportDesigner;

/// <summary>
/// Mapper class to convert ReportHeaderFooterConfig to ReportSectionModel.
/// Uses a tuple parameter to pass both the config and the section type (Header/Footer).
/// </summary>
public class ReportHeaderFooterConfigToReportSectionModelMapper : IMapType<(ReportHeaderFooterConfig config, string sectionType), ReportSectionModel>
{
    /// <summary>
    /// Maps a ReportHeaderFooterConfig to a ReportSectionModel.
    /// </summary>
    /// <param name="source">A tuple containing the config and section type (Header or Footer).</param>
    /// <returns>A ReportSectionModel with mapped data.</returns>
    public ReportSectionModel Map((ReportHeaderFooterConfig config, string sectionType) source)
    {
        var (config, sectionType) = source;

        return new ReportSectionModel
        {
            Name = config.Name,
            Description = $"{sectionType}: {config.Name}",
            HeadingText = config.Name,
            Format = sectionType,
            Type = "Absolute",
            Dynamic = false,
            Fields = new List<ReportSectionFieldModel>(),
            Grids = new List<ReportSectionGridModel>(),
            Lines = MapLines(config.Lines),
            Images = MapImages(config.Images)
        };
    }

    /// <summary>
    /// Maps the lines from ReportHeaderFooterConfig to ReportLineModel list.
    /// </summary>
    /// <param name="lines">The list of LineConfig to map.</param>
    /// <returns>A list of ReportLineModel objects.</returns>
    private static List<ReportLineModel> MapLines(List<LineConfig> lines)
    {
        if (lines == null || !lines.Any())
        {
            return new List<ReportLineModel>();
        }

        return lines.Select(l => new ReportLineModel
        {
            Line = l.Line,
            Left = l.Left,
            Field = l.Field,
            Text = l.Text,
            FontSize = l.FontSize,
            Bold = l.Bold,
            Calc = l.Calc
        }).ToList();
    }

    /// <summary>
    /// Maps the images from ReportHeaderFooterConfig to ReportImageModel list.
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
}
