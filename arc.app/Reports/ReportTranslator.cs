using arc.app.Common;
using arc.app.Config.Reports;
using arc.app.Config.Reports.DataSection;
using arc.app.Files;
using arc.app.Images;
using arc.common.ExtensionMethods;
using arc.common.Models.Reports;
using arc.common.Models.Reports.ReportDesigner;
using arc.domain.Configuration.ReportsConfig;
using arc.domain.Configuration.ReportsConfig.ReportToPrint;
using arc.domain.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using arc.app.Config.Reports.SectionFormats;

namespace arc.app.Reports;

/// <summary>
/// Translates report configurations from the database into printable report configurations.
/// </summary>
public class ReportTranslator : IReportTranslator
{
    private readonly ISectionAdapter _sectionAdapter;
    private readonly IReportHeaderAdapter _reportHeaderAdapter;
    private readonly IReportFooterAdapter _reportFooterAdapter;
    private readonly IImageRepository _imageRepository;
    private readonly IDataSectionAdapter _dataSectionAdapter;
    private readonly ISectionFormatAdapter _sectionFormatAdapter;
    private readonly ILogWriter _logWriter;
    private readonly IFileHandler _fileHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportTranslator"/> class.
    /// </summary>
    public ReportTranslator(ISectionAdapter sectionAdapter, IDataSectionAdapter dataSectionAdapter, ILogWriter logWriter, IReportHeaderAdapter reportHeaderAdapter, IReportFooterAdapter reportFooterAdapter, IImageRepository imageRepository, ISectionFormatAdapter sectionFormatAdapter, IFileHandler fileHandler)
    {
        _sectionAdapter = sectionAdapter;
        _dataSectionAdapter = dataSectionAdapter;
        _logWriter = logWriter;
        _reportHeaderAdapter = reportHeaderAdapter;
        _reportFooterAdapter = reportFooterAdapter;
        _imageRepository = imageRepository;
        _sectionFormatAdapter = sectionFormatAdapter;
        _fileHandler = fileHandler;
    }

    /// <summary>
    /// Translates the report configuration from the database into a printable report configuration.
    /// </summary>
    /// <param name="reportConfig">The report configuration.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the printable report configuration.</returns>
    public async Task<ReportToPrintConfig> TranslateAsync(ReportConfig reportConfig)
    {
        var headerConfig = string.IsNullOrEmpty(reportConfig.Header) ? new ReportHeaderFooterConfig() : await _reportHeaderAdapter.GetHeaderAsync(reportConfig.Header);
        var footerConfig = string.IsNullOrEmpty(reportConfig.Footer) ? new ReportHeaderFooterConfig() : await _reportFooterAdapter.GetFooterAsync(reportConfig.Footer);

        ReportToPrintConfig configToReturn = new()
        {
            Header = await AddImageValuesToHeaderOrFooterConfigAsync(headerConfig),
            Footer = await AddImageValuesToHeaderOrFooterConfigAsync(footerConfig)
        };

        ContentsConfig topAlert = new()
        {
            Name = "Alerts",
            Head = ["@GenAleA@"],
            Type = "Table",
            Data = "TopAlerts",
            Left = 20,
            Width = "550",
            Colour = "red"
        };
        
        ContentsConfig bottomAlert = new()
        {
            Name = "Alerts",
            Head = ["@GenAleB@"],
            Type = "Table",
            Data = "BottomAlerts",
            Left = 20,
            Width = "550",
            Colour = "lightblue"
        };

        configToReturn.Contents.Add(topAlert);

        _logWriter.LogInfo($"Get main sections for report : {reportConfig.Name}", "ReportTranslator", "Translate");
        configToReturn.Contents.AddRange(await TranslateSectionGroupAsync(reportConfig.MainSections));
        _logWriter.LogInfo($"Get organism sections for report : {reportConfig.Name}", "ReportTranslator", "Translate");
        configToReturn.Contents.AddRange(await TranslateSectionGroupAsync(reportConfig.OrganismSections, "organisms"));
        _logWriter.LogInfo($"Get final sections for report : {reportConfig.Name}", "ReportTranslator", "Translate");
        configToReturn.Contents.AddRange(await TranslateSectionGroupAsync(reportConfig.FinalSections));

        configToReturn.Contents.Add(bottomAlert);

        return configToReturn;
    }

    /// <summary>
    /// Translates a group of sections into a list of content configurations.
    /// </summary>
    /// <param name="sectionsInGroup">The sections in the group.</param>
    /// <param name="sectionType">The type of the section.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of content configurations.</returns>
    private async Task<List<ContentsConfig>> TranslateSectionGroupAsync(List<string> sectionsInGroup, string sectionType = "")
    {
        List<ContentsConfig> returnList = [];
        var firstSection = true;

        foreach (var section in sectionsInGroup)
        {
            _logWriter.LogInfo($"Get section definition for report, section : {section}", "TranslateSectionGroup", "Translate");
            var sectionConfig = await _sectionAdapter.GetSectionAsync(section); // load here fails
            _logWriter.LogInfo($"Get data section definition for report, data section : {sectionConfig.DataSection}", "TranslateSectionGroup", "Translate");
            var dataSectionConfig = sectionConfig.DataSection == null ? null : await _dataSectionAdapter.GetSectionAsync(sectionConfig.DataSection);

            var sectionDefinition = await GetSectionDefinitionAsync(sectionConfig.Format);
            var format = await _sectionFormatAdapter.GetFormatAsync(sectionConfig.Format);

            var formatHeadingEnabled = ReportSectionHeadingExtensions.FormatHeadingEnabled(format?.Heading?.Count ?? 0);
            var formatHeading = formatHeadingEnabled ? format.Heading[0] : null;
            var printHeading = sectionConfig.HeadingText.BuildPrintHeadingLine(
                formatHeadingEnabled,
                formatHeading?.Line ?? 0,
                formatHeading?.Left ?? 0,
                formatHeading?.FontSize ?? 0,
                formatHeading?.Bold ?? false);

            if (printHeading == null)
            {
                _logWriter.LogInfo(
                    $"Section {sectionConfig.Name} has no HeadingText; section heading omitted from printable config",
                    nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
            }
            else if (formatHeadingEnabled)
            {
                _logWriter.LogInfo(
                    $"Section {sectionConfig.Name} heading uses format {format.Name} geometry (Left={printHeading.Left}, FontSize={printHeading.FontSize})",
                    nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
            }
            else
            {
                _logWriter.LogInfo(
                    $"Section {sectionConfig.Name} heading uses default geometry; format {format?.Name ?? sectionConfig.Format} heading slot disabled",
                    nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
            }

            ContentsConfig newConfig = new()
            {
                Name = sectionConfig.Name,
                Heading = printHeading == null
                    ? null
                    :
                    [
                        new()
                        {
                            Line = printHeading.Line,
                            Left = printHeading.Left,
                            Text = printHeading.Text,
                            FontSize = printHeading.FontSize,
                            Bold = printHeading.Bold
                        }
                    ],
                Type = sectionDefinition.Type,
                Dynamic = sectionConfig.Dynamic,
                Separator = sectionConfig.Separator
            };

            if (sectionType == "organisms")
            {
                newConfig.Group = "Organism";
                if (firstSection)
                {
                    newConfig.Multiple = true;
                    newConfig.MultipleName = "Organisms";
                    newConfig.LinkedSections = sectionsInGroup.Count - 1;
                }
            }

            if (sectionDefinition.Columns != null)
            {
                _logWriter.LogInfo($"Get column definitions for section : {section}", "TranslateSectionGroup", "Translate");
                var columnNumber = 1;
                foreach (var column in sectionDefinition.Columns)
                {
                    switch (columnNumber)
                    {
                        case 1:
                            newConfig.Column1 = DefineColumn(sectionConfig.Fields, columnNumber, column.Left, column.Width, column.LabelWidth, sectionConfig.Name);
                            break;
                        case 2:
                            newConfig.Column2 = DefineColumn(sectionConfig.Fields, columnNumber, column.Left, column.Width, column.LabelWidth, sectionConfig.Name);
                            break;
                        default:
                            break;
                    }
                    columnNumber++;
                }

                var noBoxFields = sectionConfig.Fields?
                    .Where(f => f.NoBox && !string.IsNullOrWhiteSpace(f.Value))
                    .Select(f => f.Value)
                    .ToList();
                if (noBoxFields?.Count > 0)
                {
                    _logWriter.LogInfo(
                        $"Section {sectionConfig.Name} has {noBoxFields.Count} field(s) with NoBox: {string.Join(", ", noBoxFields)}",
                        nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                }
            }

            // One table block follows per grid the section binds, so that is what the layout section links to.
            var boundGridCount = sectionConfig.Grids?.Count ?? 0;
            if (sectionDefinition.Grids != null && boundGridCount > 0 && sectionDefinition.Type != "Table")
            {
                newConfig.LinkedSections = boundGridCount;
            }

            if (sectionConfig.Lines?.Count > 0)
            {
                newConfig.Lines = sectionConfig.Lines;
            }

            if (sectionConfig.Images?.Count > 0)
            {
                newConfig.Images = await AddImageValuesToImageConfigsAsync(sectionConfig.Images);
            }

            // Collected per section rather than appended straight to the return list, because the area row
            // arrangement may reorder them and rewrite their geometry before they are emitted.
            List<ContentsConfig> sectionEntries = [];
            Dictionary<string, ContentsConfig> gridEntriesByGridId = new(StringComparer.Ordinal);

            var thisSectionOnlyContainsATable = true;
            if (sectionDefinition.Type != "Table")
            {
                sectionEntries.Add(newConfig);
                thisSectionOnlyContainsATable = false;
            }
            firstSection = false;

            //Add any grid sections
            if (sectionDefinition.Grids != null && dataSectionConfig != null)
            {
                var formatGrids = sectionDefinition.Grids;
                var sectionGrids = sectionConfig.Grids ?? new List<ReportSectionGridConfig>();
                var dataSectionGrids = dataSectionConfig.Grids ?? new List<DataSectionGridConfig>();

                if (dataSectionGrids.Count > sectionGrids.Count)
                {
                    _logWriter.LogInfo($"Section {sectionConfig.Name} places {sectionGrids.Count} of the {dataSectionGrids.Count} grid(s) offered by data section {dataSectionConfig.Name}", nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                }

                if (formatGrids.Count > 0 && sectionGrids.Count > formatGrids.Count)
                {
                    _logWriter.LogInfo($"Section {sectionConfig.Name} binds {sectionGrids.Count} grid(s) but format {sectionDefinition.Name} defines {formatGrids.Count} grid position(s); the last position's geometry is reused for the remainder", nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                }

                // Driven by the section's bindings rather than the format's positions, so a section that
                // places more grids than its format defines still renders all of them.
                for (var gridIndex = 0; gridIndex < sectionGrids.Count; gridIndex++)
                {
                    var formatGrid = GetFormatGridForIndex(formatGrids, gridIndex);
                    var sectionGrid = sectionGrids[gridIndex];

                    if (formatGrid == null)
                    {
                        _logWriter.LogInfo($"Format {sectionDefinition.Name} defines no grid positions, unable to place grid '{sectionGrid.Name}' for section {sectionConfig.Name}", nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                        continue;
                    }

                    // Matched on the grid id, never on the description, so the binding survives translation.
                    var dataGridDefinition = dataSectionGrids.FirstOrDefault(g => g.Name.IsSameGridId(sectionGrid.Name));
                    if (dataGridDefinition == null)
                    {
                        _logWriter.LogInfo($"Unable to find data grid '{sectionGrid.Name}' for section {sectionConfig.Name}", nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                        continue;
                    }

                    var rawHeadings = sectionGrid.Head ?? new List<string>();
                    var hasAnyHeading = rawHeadings.Any(h => !string.IsNullOrWhiteSpace(h));
                    // Preserve positional blank entries so that a heading set like ["", "Col2", "Col3"]
                    // maps correctly to all three columns. Head is only set to null when every entry is blank.
                    var columnHeadings = hasAnyHeading
                        ? rawHeadings.Select(h => h?.Trim() ?? string.Empty).ToList()
                        : new List<string>();

                    var normalizedWidth = NormalizeGridWidth(formatGrid.Width, columnHeadings.Count, sectionConfig.Name, gridIndex);

                    _logWriter.LogInfo($"Section {sectionConfig.Name} grid {gridIndex + 1} placed from '{sectionGrid.Name}' reading data key '{dataGridDefinition.Data}' with {columnHeadings.Count} heading(s) and width '{normalizedWidth}'", nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                    ContentsConfig gridConfig = new()
                    {
                        Name = dataGridDefinition.Name,
                        Data = dataGridDefinition.Data,
                        Left = formatGrid.Left,
                        Width = normalizedWidth,
                        Type = "Table",
                        Theme = "grid",
                        Group = thisSectionOnlyContainsATable ? newConfig.Group : sectionType == "organisms" ? "Organism" : null,
                        Head = columnHeadings.Any() ? columnHeadings : null,
                        Colour = sectionConfig.Description.Contains("Comments") ? "manatee" : "#287fba",
                        NoBox = sectionGrid.NoBox
                    };

                    if (sectionGrid.NoBox)
                    {
                        _logWriter.LogInfo(
                            $"Section {sectionConfig.Name} grid '{sectionGrid.Name}' has NoBox; cell borders omitted from printable config",
                            nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                    }

                    if (thisSectionOnlyContainsATable && gridIndex == 0 && printHeading != null)
                    {
                        gridConfig.Heading = newConfig.Heading;
                        _logWriter.LogInfo(
                            $"Section {sectionConfig.Name} is grid-only; section heading attached to first grid '{sectionGrid.Name}'",
                            nameof(ReportTranslator), nameof(TranslateSectionGroupAsync));
                    }

                    sectionEntries.Add(gridConfig);
                    gridEntriesByGridId[sectionGrid.Name.NormalisedGridId()] = gridConfig;
                }
            }

            returnList.AddRange(ApplyLayoutRows(
                sectionConfig,
                thisSectionOnlyContainsATable ? null : newConfig,
                gridEntriesByGridId,
                sectionEntries));
        }

        return returnList;
    }

    /// <summary>
    /// Arranges a section's printable entries into its area rows, rewriting the geometry of any areas that
    /// share a row so they sit side by side instead of on top of one another.
    /// </summary>
    /// <param name="sectionConfig">The section being translated.</param>
    /// <param name="fieldBlockEntry">The section's field block entry, or null for a grid-only section.</param>
    /// <param name="gridEntriesByGridId">The section's grid entries, keyed by normalised section grid id.</param>
    /// <param name="sectionEntries">The section's entries in their default stacked order.</param>
    /// <returns>
    /// The entries in row order. A section with no stored arrangement, or one whose rows each hold a single
    /// area, comes back untouched and therefore renders exactly as it did before area rows existed.
    /// </returns>
    private List<ContentsConfig> ApplyLayoutRows(
        ReportSectionConfig sectionConfig,
        ContentsConfig fieldBlockEntry,
        Dictionary<string, ContentsConfig> gridEntriesByGridId,
        List<ContentsConfig> sectionEntries)
    {
        var resolvedRows = sectionConfig.LayoutRows.ResolveLayoutRows(sectionConfig.Grids, fieldBlockEntry != null);

        if (!resolvedRows.HasSideBySideRow())
        {
            _logWriter.LogInfo(
                $"Section {sectionConfig.Name} has no side-by-side area row; {sectionEntries.Count} area(s) render stacked in document order",
                nameof(ReportTranslator), nameof(ApplyLayoutRows));
            return sectionEntries;
        }

        var rows = new List<List<ContentsConfig>>();
        var arranged = new List<ContentsConfig>();

        for (var rowIndex = 0; rowIndex < resolvedRows.Count; rowIndex++)
        {
            var rowEntries = new List<ContentsConfig>();
            var rowPercents = new List<int>();

            foreach (var area in resolvedRows[rowIndex].Areas)
            {
                var entry = LayoutAreaTypes.IsFieldsArea(area.Type)
                    ? fieldBlockEntry
                    : gridEntriesByGridId.GetValueOrDefault(area.Name.NormalisedGridId());

                if (entry == null)
                {
                    _logWriter.LogWarning(
                        $"Section {sectionConfig.Name} row {rowIndex + 1} references area '{area.Name ?? area.Type}' which has no printable entry; the area is dropped from the row",
                        nameof(ReportTranslator), nameof(ApplyLayoutRows));
                    continue;
                }

                rowEntries.Add(entry);
                rowPercents.Add(area.WidthPercent);
            }

            if (rowEntries.Count == 0)
            {
                continue;
            }

            rows.Add(rowEntries);
            arranged.AddRange(rowEntries);

            if (rowEntries.Count > 1)
            {
                ApplyRowGeometry(sectionConfig, rowIndex, rowEntries, rowPercents, fieldBlockEntry);
            }
        }

        // Anything the arrangement did not mention still has to print, or a grid would silently disappear.
        arranged.AddRange(sectionEntries.Where(entry => !arranged.Contains(entry)));

        MoveGridOnlyHeadingToFirstEntry(sectionConfig, fieldBlockEntry, sectionEntries, arranged);

        return arranged;
    }

    /// <summary>
    /// Rewrites the geometry of the areas on one row so they divide the row's natural span between them.
    /// </summary>
    /// <param name="sectionConfig">The section being translated.</param>
    /// <param name="rowIndex">Zero based index of the row within the section.</param>
    /// <param name="rowEntries">The entries sharing the row, ordered left to right.</param>
    /// <param name="rowPercents">Each entry's requested share of the row width, where zero means an equal share.</param>
    /// <param name="fieldBlockEntry">The section's field block entry, used to spot when the field block is on this row.</param>
    /// <remarks>
    /// Every value is staged and only committed once the whole row is known to fit, so a row that cannot be
    /// laid out is left completely untouched and falls back to stacked rendering rather than printing half
    /// rescaled.
    /// </remarks>
    private void ApplyRowGeometry(
        ReportSectionConfig sectionConfig,
        int rowIndex,
        List<ContentsConfig> rowEntries,
        List<int> rowPercents,
        ContentsConfig fieldBlockEntry)
    {
        var naturalBounds = rowEntries.Select(GetNaturalBounds).ToList();
        var spanLeft = naturalBounds.Min(bounds => bounds.Left);
        var spanRight = naturalBounds.Max(bounds => bounds.Right);

        var slots = ReportSectionLayoutExtensions.ComputeRowSlots(spanLeft, spanRight, rowPercents);
        if (slots == null)
        {
            _logWriter.LogWarning(
                $"Section {sectionConfig.Name} row {rowIndex + 1} cannot fit {rowEntries.Count} areas across {spanRight - spanLeft}pt (span {spanLeft}-{spanRight}); row falls back to stacked rendering",
                nameof(ReportTranslator), nameof(ApplyRowGeometry));
            return;
        }

        var scaledGridWidths = new string[rowEntries.Count];

        for (var index = 0; index < rowEntries.Count; index++)
        {
            if (rowEntries[index] == fieldBlockEntry)
            {
                continue;
            }

            scaledGridWidths[index] = rowEntries[index].Width.ScaleGridWidthDefinition(slots[index].Width);

            if (scaledGridWidths[index] == null)
            {
                _logWriter.LogWarning(
                    $"Section {sectionConfig.Name} row {rowIndex + 1} grid '{rowEntries[index].Name}' cannot scale widths '{rowEntries[index].Width}' into {slots[index].Width}pt without a column falling below {ReportSectionLayoutExtensions.MinimumGridColumnWidth}pt; row falls back to stacked rendering",
                    nameof(ReportTranslator), nameof(ApplyRowGeometry));
                return;
            }
        }

        var rowKey = sectionConfig.Name.LayoutRowKey(rowIndex);

        for (var index = 0; index < rowEntries.Count; index++)
        {
            var entry = rowEntries[index];
            entry.LayoutRowKey = rowKey;
            entry.LayoutRowAreaCount = rowEntries.Count;

            if (entry == fieldBlockEntry)
            {
                ScaleFieldBlockIntoSlot(entry, naturalBounds[index], slots[index]);

                // The deferred heading hands a mixed section's heading to its first grid, which assumes the
                // grid prints below the fields. On a shared row the heading belongs with the field block.
                entry.LinkedSections = 0;
            }
            else
            {
                entry.Left = slots[index].Left;
                entry.Width = scaledGridWidths[index];
            }
        }

        _logWriter.LogInfo(
            $"Section {sectionConfig.Name} row {rowIndex + 1} places {rowEntries.Count} areas side by side across span {spanLeft}-{spanRight}pt: {DescribeRowGeometry(rowEntries, slots, fieldBlockEntry)}",
            nameof(ReportTranslator), nameof(ApplyRowGeometry));
    }

    /// <summary>
    /// Rescales a field block's columns into the slot the block was given on a shared row.
    /// </summary>
    /// <param name="entry">The field block entry to rewrite.</param>
    /// <param name="naturalBounds">The block's natural horizontal extent before scaling.</param>
    /// <param name="slot">The slot the block was allocated.</param>
    private static void ScaleFieldBlockIntoSlot(
        ContentsConfig entry,
        ReportSectionLayoutSlotModel naturalBounds,
        ReportSectionLayoutSlotModel slot)
    {
        foreach (var column in new[] { entry.Column1, entry.Column2 })
        {
            if (column == null)
            {
                continue;
            }

            var scaled = ReportSectionLayoutExtensions.ScaleFieldColumn(
                column.Left,
                column.Width,
                column.LabelWidth,
                naturalBounds.Left,
                naturalBounds.Width,
                slot);

            column.Left = scaled.Left;
            column.Width = scaled.Width;
            column.LabelWidth = scaled.LabelWidth;
        }
    }

    /// <summary>
    /// Reads the horizontal extent a printable entry occupies before any row scaling.
    /// </summary>
    /// <param name="entry">A field block or grid entry.</param>
    /// <returns>
    /// The entry's left edge and width. A grid measures its pipe delimited column widths from its Left; a
    /// field block spans from its leftmost column to the right edge of its rightmost column.
    /// </returns>
    private static ReportSectionLayoutSlotModel GetNaturalBounds(ContentsConfig entry)
    {
        var columns = new[] { entry.Column1, entry.Column2 }.Where(column => column != null).ToList();

        if (columns.Count > 0)
        {
            var left = columns.Min(column => column.Left);
            var right = columns.Max(column => column.Left + column.Width);

            return new ReportSectionLayoutSlotModel { Left = left, Width = right - left };
        }

        return new ReportSectionLayoutSlotModel
        {
            Left = entry.Left,
            Width = entry.Width.ParseGridWidths().Sum()
        };
    }

    /// <summary>
    /// Moves a grid-only section's heading onto whichever grid the arrangement put first.
    /// </summary>
    /// <param name="sectionConfig">The section being translated.</param>
    /// <param name="fieldBlockEntry">The section's field block entry, or null when the section is grid-only.</param>
    /// <param name="sectionEntries">The entries in their default stacked order.</param>
    /// <param name="arranged">The entries in row order.</param>
    /// <remarks>
    /// The heading is attached to the first grid while the entries are built, which is correct until the
    /// arrangement reorders them. Left alone, the heading would print part way down a grid-only section.
    /// </remarks>
    private void MoveGridOnlyHeadingToFirstEntry(
        ReportSectionConfig sectionConfig,
        ContentsConfig fieldBlockEntry,
        List<ContentsConfig> sectionEntries,
        List<ContentsConfig> arranged)
    {
        if (fieldBlockEntry != null || sectionEntries.Count == 0 || arranged.Count == 0)
        {
            return;
        }

        var headingHolder = sectionEntries[0];
        if (headingHolder.Heading == null || ReferenceEquals(headingHolder, arranged[0]))
        {
            return;
        }

        arranged[0].Heading = headingHolder.Heading;
        headingHolder.Heading = null;

        _logWriter.LogInfo(
            $"Section {sectionConfig.Name} is grid-only and rearranged; section heading moved to grid '{arranged[0].Name}'",
            nameof(ReportTranslator), nameof(MoveGridOnlyHeadingToFirstEntry));
    }

    /// <summary>
    /// Describes a row's computed geometry for the log, so an installed system's layout can be reconstructed
    /// from its log file without attaching a debugger.
    /// </summary>
    /// <param name="rowEntries">The entries sharing the row.</param>
    /// <param name="slots">The slot each entry was allocated.</param>
    /// <param name="fieldBlockEntry">The section's field block entry.</param>
    /// <returns>A comma separated description of each area's identity, left edge and width.</returns>
    private static string DescribeRowGeometry(
        List<ContentsConfig> rowEntries,
        List<ReportSectionLayoutSlotModel> slots,
        ContentsConfig fieldBlockEntry)
    {
        return string.Join(", ", rowEntries.Select((entry, index) =>
        {
            var identity = entry == fieldBlockEntry ? "fields" : $"grid '{entry.Name}'";
            return $"{identity} Left={slots[index].Left} Width={slots[index].Width}";
        }));
    }

    /// <summary>
    /// Picks the format grid that supplies the geometry for a section's grid binding.
    /// </summary>
    /// <param name="formatGrids">The grid positions the section's format defines.</param>
    /// <param name="gridIndex">The zero based index of the binding being placed.</param>
    /// <returns>
    /// The format grid at the same index, or the last one defined where the section binds more grids
    /// than the format has positions, or null when the format defines no grid positions at all.
    /// </returns>
    private GridDefinitionModel GetFormatGridForIndex(List<GridDefinitionModel> formatGrids, int gridIndex)
    {
        if (formatGrids == null || formatGrids.Count == 0)
        {
            return null;
        }

        return gridIndex < formatGrids.Count ? formatGrids[gridIndex] : formatGrids[^1];
    }

    /// <summary>
    /// Defines a column configuration based on the provided fields and column properties.
    /// </summary>
    /// <param name="fields">The fields in the column.</param>
    /// <param name="columnNumber">The column number.</param>
    /// <param name="left">The left margin of the column.</param>
    /// <param name="width">The width of the column.</param>
    /// <param name="labelWidth">The label width of the column.</param>
    /// <returns>The column configuration.</returns>
    private ColumnConfig DefineColumn(List<ReportSectionFieldConfig> fields, int columnNumber, int left, int width, int labelWidth, string sectionName = null)
    {
        var columnFields = fields
            .Where(f => f.Column == columnNumber)
            .OrderBy(o => o.Order)
            .ToList();

        var noBoxCount = columnFields.Count(f => f.NoBox);
        if (noBoxCount > 0 && noBoxCount < columnFields.Count && !string.IsNullOrWhiteSpace(sectionName))
        {
            _logWriter.LogInfo(
                $"Section {sectionName} column {columnNumber} mixes boxed and NoBox fields ({noBoxCount} of {columnFields.Count} without borders)",
                nameof(ReportTranslator), nameof(DefineColumn));
        }

        return new ColumnConfig
        {
            Left = left,
            Width = width,
            LabelWidth = labelWidth,
            Fields = [.. columnFields
            .Select(s => new FieldConfig {
                Label = s.Label,
                Value = GetFieldValueAsync(s.Value, s.Image).Result,
                Image = s.Image,
                Width = s.Width,
                Height = s.Height,
                Format = s.Format,
                Text = s.Text,
                NoBox = s.NoBox
            })]
        };
    }

    /// <summary>
    /// Extracts the image format from a content type string.
    /// </summary>
    /// <param name="contentType">The content type (e.g., "image/png").</param>
    /// <returns>The image format extension (e.g., "png").</returns>
    private string GetImageFormatFromContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return "png";
        }

        return contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => "jpeg",
            "image/jpg" => "jpg",
            "image/png" => "png",
            "image/gif" => "gif",
            "image/bmp" => "bmp",
            "image/webp" => "webp",
            _ => contentType.Replace("image/", "").ToLowerInvariant()
        };
    }

    /// <summary>
    /// Loads image data from file storage and converts it to base64.
    /// </summary>
    /// <param name="fileAttachmentId">The file attachment ID.</param>
    /// <returns>A tuple containing the base64 string and format, or null values if the image cannot be loaded.</returns>
    private async Task<(string base64String, string format)> LoadImageDataAsync(int fileAttachmentId)
    {
        try
        {
            var fileData = await _fileHandler.ReadByIdAsync(fileAttachmentId);
            if (fileData?.Bytes != null && fileData.Bytes.Length > 0)
            {
                var base64String = Convert.ToBase64String(fileData.Bytes);
                var format = GetImageFormatFromContentType(fileData.ContentType);
                return (base64String, format);
            }
        }
        catch (Exception ex)
        {
            _logWriter.LogError($"Error loading image from file attachment {fileAttachmentId}: {ex.Message}", nameof(ReportTranslator), nameof(LoadImageDataAsync));
        }

        return (string.Empty, "png");
    }

    /// <summary>
    /// Retrieves the field value asynchronously, converting it to a base64 string if it is an image.
    /// </summary>
    /// <param name="value">The field value (image name if it's an image field).</param>
    /// <param name="image">Indicates whether the field is an image.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the field value (base64 string for images).</returns>
    private async Task<string> GetFieldValueAsync(string value, bool image)
    {
        if (image)
        {
            var reportImage = await _imageRepository.GetImageAsync(value);
            if (reportImage?.FileAttachmentId > 0)
            {
                var (base64String, _) = await LoadImageDataAsync(reportImage.FileAttachmentId);
                return base64String;
            }

            _logWriter.LogInfo($"Image '{value}' not found or has no file attachment", nameof(ReportTranslator), nameof(GetFieldValueAsync));
            return string.Empty;
        }
        return value;
    }

    /// <summary>
    /// Normalises a pipe-delimited column width definition against the expected
    /// number of column headings, logging a warning when the counts differ.
    /// </summary>
    /// <param name="widthDefinition">
    /// A pipe-delimited string of integer column widths (e.g. "60|80|100").
    /// Returned unchanged if null or whitespace.
    /// </param>
    /// <param name="headingCount">
    /// The number of column headings for the grid, including positional blank entries.
    /// Used to validate that the width count matches the heading count.
    /// </param>
    /// <param name="sectionName">
    /// The name of the report section, used in log messages.
    /// </param>
    /// <param name="gridIndex">
    /// The zero-based index of the grid within the section, used in log messages.
    /// </param>
    /// <returns>
    /// A pipe-delimited string of the parsed integer widths, or the original
    /// <paramref name="widthDefinition"/> if no valid integers could be parsed.
    /// </returns>
    private string NormalizeGridWidth(string widthDefinition, int headingCount, string sectionName, int gridIndex)
    {
        if (string.IsNullOrWhiteSpace(widthDefinition))
        {
            return widthDefinition;
        }

        var parsedWidths = new List<int>();
        var segments = widthDefinition.Split('|', StringSplitOptions.RemoveEmptyEntries);
        foreach (var segment in segments)
        {
            var trimmedSegment = segment.Trim();
            if (int.TryParse(trimmedSegment, out var parsedWidth))
            {
                parsedWidths.Add(parsedWidth);
            }
            else
            {
                _logWriter.LogInfo($"Invalid grid width '{trimmedSegment}' for section {sectionName} grid {gridIndex + 1}", nameof(ReportTranslator), nameof(NormalizeGridWidth));
            }
        }

        if (headingCount > 0 && parsedWidths.Count > 0 && headingCount != parsedWidths.Count)
        {
            _logWriter.LogInfo($"Grid width count mismatch for section {sectionName} grid {gridIndex + 1}: headings {headingCount}, widths {parsedWidths.Count}", nameof(ReportTranslator), nameof(NormalizeGridWidth));
        }

        return parsedWidths.Count > 0
            ? string.Join("|", parsedWidths)
            : widthDefinition;
    }
    /// <summary>
    /// Loads image data from file storage and populates the image configurations with base64 values.
    /// </summary>
    /// <param name="imageConfigs">The list of image configurations.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the list of image configurations with populated values.</returns>
    private async Task<List<ImageConfig>> AddImageValuesToImageConfigsAsync(List<ImageConfig> imageConfigs)
    {
        foreach (var imageConfig in imageConfigs)
        {
            var imageMetadata = await _imageRepository.GetImageAsync(imageConfig.Name);
            if (imageMetadata?.FileAttachmentId > 0)
            {
                var (base64String, format) = await LoadImageDataAsync(imageMetadata.FileAttachmentId);
                imageConfig.Value = base64String;
                imageConfig.Format = format;
            }
            else
            {
                _logWriter.LogInfo($"Image '{imageConfig.Name}' not found or has no file attachment", nameof(ReportTranslator), nameof(AddImageValuesToImageConfigsAsync));
                imageConfig.Value = string.Empty;
                imageConfig.Format = "png";
            }
        }
        return imageConfigs;
    }

    /// <summary>
    /// Adds image values to the header or footer configuration.
    /// </summary>
    /// <param name="headerFooterConfig">The header or footer configuration.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated header or footer configuration.</returns>
    private async Task<ReportHeaderFooterConfig> AddImageValuesToHeaderOrFooterConfigAsync(ReportHeaderFooterConfig headerFooterConfig)
    {
        headerFooterConfig.Images = await AddImageValuesToImageConfigsAsync(headerFooterConfig.Images);
        return headerFooterConfig;
    }

    /// <summary>
    /// Retrieves the section definition based on the section type.
    /// </summary>
    /// <param name="sectionType">The type of the section.</param>
    /// <returns>The section definition model.</returns>
    public async Task<SectionDefinitionModel> GetSectionDefinitionAsync(string sectionType)
    {
        var format = await _sectionFormatAdapter.GetFormatAsync(sectionType);
        return new SectionDefinitionModel
        {
            Name = format.Name,
            Type = format.Type,
            Columns = format.Columns?.Select(c => new ColumnDefinitionModel { Left = c.Left, Width = c.Width, LabelWidth = c.LabelWidth }).ToList(),
            Grids = format.Grids?.Select(g => new GridDefinitionModel { Left = g.Left, Width = g.Width }).ToList()
        };
    }
}
