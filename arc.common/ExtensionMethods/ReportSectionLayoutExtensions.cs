using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using arc.common.Models.Reports.ReportDesigner;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Geometry for placing a layout section's field block and grids side by side on a row.
    /// </summary>
    /// <remarks>
    /// A layout section's horizontal geometry normally comes straight from its format: field columns from
    /// Columns[].Left/Width/LabelWidth and grids from Grids[].Left/Width. Those positions assume each area
    /// gets the full width to itself, so two areas placed on one row would overlap. In the seeded
    /// DoubleColumnWithTwoGrids format both grid positions are identical (left 100, width "300|80"), which
    /// would draw the second grid exactly on top of the first.
    /// <para>
    /// These helpers divide a row's natural span into one slot per area and rescale each area's own geometry
    /// into its slot. A row never reaches beyond the span its areas already occupied, so putting areas side
    /// by side cannot push a section past the format's footprint or off the page.
    /// </para>
    /// <para>
    /// Everything here works on primitives so that the print translator (arc.app, working on domain configs)
    /// and the save path (arc.data, working on designer models) can share one implementation.
    /// The JavaScript twin used by the designer preview is
    /// arcportal/src/components/Reports/Functions/layoutRowGeometry.js and must stay in step.
    /// </para>
    /// </remarks>
    public static class ReportSectionLayoutExtensions
    {
        /// <summary>
        /// Gap in points left between two areas that share a row.
        /// </summary>
        public const int RowAreaGutter = 10;

        /// <summary>
        /// Narrowest grid column in points. Anything narrower cannot hold a readable value once scaled.
        /// </summary>
        public const int MinimumGridColumnWidth = 12;

        /// <summary>
        /// Narrowest slot in points an area may be given. A row that cannot give every area this much is
        /// rendered stacked instead.
        /// </summary>
        public const int MinimumAreaWidth = 40;

        /// <summary>
        /// Narrowest value column in points left over after a field column's label column is scaled.
        /// </summary>
        public const int MinimumFieldValueWidth = 16;

        /// <summary>
        /// Builds the row grouping key that tags every printable entry belonging to one row.
        /// </summary>
        /// <param name="sectionName">The section the row belongs to.</param>
        /// <param name="rowIndex">Zero based row index within the section.</param>
        /// <returns>A key unique to this section and row, so two sections cannot share a row by accident.</returns>
        public static string LayoutRowKey(this string sectionName, int rowIndex)
        {
            return $"{sectionName.NormalisedGridId()}#{rowIndex}";
        }

        /// <summary>
        /// Divides a row's horizontal span into one slot per area.
        /// </summary>
        /// <param name="spanLeft">Leftmost point of the row span, taken from the areas' natural geometry.</param>
        /// <param name="spanRight">First point beyond the right edge of the row span.</param>
        /// <param name="widthPercents">
        /// Each area's requested share of the row, 1 to 100. A zero entry takes an equal share of whatever the
        /// areas with an explicit share leave behind.
        /// </param>
        /// <returns>
        /// One slot per area, ordered left to right, or null when the span cannot give every area at least
        /// <see cref="MinimumAreaWidth"/> points.
        /// </returns>
        public static List<ReportSectionLayoutSlotModel> ComputeRowSlots(
            int spanLeft,
            int spanRight,
            IReadOnlyList<int> widthPercents)
        {
            var areaCount = widthPercents?.Count ?? 0;
            if (areaCount == 0)
            {
                return [];
            }

            var availableWidth = spanRight - spanLeft - (RowAreaGutter * (areaCount - 1));
            if (availableWidth < MinimumAreaWidth * areaCount)
            {
                return null;
            }

            var shares = NormaliseWidthShares(widthPercents);
            var widths = new int[areaCount];

            for (var index = 0; index < areaCount; index++)
            {
                widths[index] = (int)Math.Round(availableWidth * shares[index] / 100m, MidpointRounding.AwayFromZero);
            }

            AbsorbRoundingDrift(widths, availableWidth);

            if (widths.Any(width => width < MinimumAreaWidth))
            {
                return null;
            }

            var slots = new List<ReportSectionLayoutSlotModel>(areaCount);
            var left = spanLeft;

            for (var index = 0; index < areaCount; index++)
            {
                slots.Add(new ReportSectionLayoutSlotModel { Left = left, Width = widths[index] });
                left += widths[index] + RowAreaGutter;
            }

            return slots;
        }

        /// <summary>
        /// Rescales a pipe delimited grid width definition so its columns add up to a target width.
        /// </summary>
        /// <param name="widthDefinition">Natural column widths from the format, such as "150|150|80".</param>
        /// <param name="targetWidth">Total width in points the scaled columns must add up to.</param>
        /// <returns>
        /// Scaled pipe delimited widths summing exactly to <paramref name="targetWidth"/>, or null when any
        /// column would fall below <see cref="MinimumGridColumnWidth"/> or the definition cannot be read.
        /// </returns>
        public static string ScaleGridWidthDefinition(this string widthDefinition, int targetWidth)
        {
            var naturalWidths = ParseGridWidths(widthDefinition);
            if (naturalWidths.Count == 0 || targetWidth <= 0)
            {
                return null;
            }

            var naturalTotal = naturalWidths.Sum();
            if (naturalTotal <= 0)
            {
                return null;
            }

            if (targetWidth < MinimumGridColumnWidth * naturalWidths.Count)
            {
                return null;
            }

            var scaled = naturalWidths
                .Select(width => (int)Math.Round(targetWidth * (decimal)width / naturalTotal, MidpointRounding.AwayFromZero))
                .ToArray();

            AbsorbRoundingDrift(scaled, targetWidth);

            return scaled.Any(width => width < MinimumGridColumnWidth)
                ? null
                : string.Join("|", scaled.Select(width => width.ToString(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Rescales one field column's geometry into the slot its field block was given on a shared row.
        /// </summary>
        /// <param name="naturalLeft">The column's Left from the format.</param>
        /// <param name="naturalWidth">The column's total Width from the format.</param>
        /// <param name="naturalLabelWidth">The column's LabelWidth from the format.</param>
        /// <param name="naturalSpanLeft">Leftmost point of the whole field block's natural geometry.</param>
        /// <param name="naturalSpanWidth">Width of the whole field block's natural geometry.</param>
        /// <param name="slot">The slot the field block was allocated on the row.</param>
        /// <returns>
        /// The column's Left, Width and LabelWidth inside the slot. Relative spacing between the format's
        /// columns is preserved, so a two column block stays a two column block, just narrower.
        /// </returns>
        public static ReportSectionLayoutSlotModel ScaleFieldColumn(
            int naturalLeft,
            int naturalWidth,
            int naturalLabelWidth,
            int naturalSpanLeft,
            int naturalSpanWidth,
            ReportSectionLayoutSlotModel slot)
        {
            if (slot == null || naturalSpanWidth <= 0)
            {
                return new ReportSectionLayoutSlotModel
                {
                    Left = naturalLeft,
                    Width = naturalWidth,
                    LabelWidth = naturalLabelWidth
                };
            }

            var scale = (decimal)slot.Width / naturalSpanWidth;
            var scaledWidth = Math.Max(
                MinimumFieldValueWidth + 1,
                (int)Math.Round(naturalWidth * scale, MidpointRounding.AwayFromZero));
            var scaledLabelWidth = (int)Math.Round(naturalLabelWidth * scale, MidpointRounding.AwayFromZero);

            return new ReportSectionLayoutSlotModel
            {
                Left = slot.Left + (int)Math.Round((naturalLeft - naturalSpanLeft) * scale, MidpointRounding.AwayFromZero),
                Width = scaledWidth,
                LabelWidth = Math.Clamp(scaledLabelWidth, 1, scaledWidth - MinimumFieldValueWidth)
            };
        }

        /// <summary>
        /// Reads a pipe delimited grid width definition into its column widths.
        /// </summary>
        /// <param name="widthDefinition">A pipe delimited width string such as "150|150|80".</param>
        /// <returns>The column widths, or an empty list when the definition is missing or unreadable.</returns>
        public static List<int> ParseGridWidths(this string widthDefinition)
        {
            if (string.IsNullOrWhiteSpace(widthDefinition))
            {
                return [];
            }

            var widths = new List<int>();

            foreach (var entry in widthDefinition.Split('|', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!decimal.TryParse(entry.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var width))
                {
                    return [];
                }

                widths.Add((int)Math.Round(width, MidpointRounding.AwayFromZero));
            }

            return widths;
        }

        /// <summary>
        /// Turns requested width percentages into shares that add up to 100.
        /// </summary>
        /// <param name="widthPercents">Requested shares, where zero means "an equal share of the remainder".</param>
        /// <returns>One share per area, summing to 100.</returns>
        private static decimal[] NormaliseWidthShares(IReadOnlyList<int> widthPercents)
        {
            var areaCount = widthPercents.Count;
            var shares = new decimal[areaCount];
            var automaticCount = widthPercents.Count(percent => percent <= 0);

            if (automaticCount == areaCount)
            {
                for (var index = 0; index < areaCount; index++)
                {
                    shares[index] = 100m / areaCount;
                }

                return shares;
            }

            var explicitTotal = widthPercents.Where(percent => percent > 0).Sum(percent => Math.Min(percent, 100));

            if (automaticCount == 0)
            {
                for (var index = 0; index < areaCount; index++)
                {
                    shares[index] = 100m * Math.Min(widthPercents[index], 100) / explicitTotal;
                }

                return shares;
            }

            var cappedExplicitTotal = Math.Min(explicitTotal, 100 - automaticCount);
            var automaticShare = (100m - cappedExplicitTotal) / automaticCount;

            for (var index = 0; index < areaCount; index++)
            {
                shares[index] = widthPercents[index] > 0
                    ? cappedExplicitTotal * Math.Min(widthPercents[index], 100) / explicitTotal
                    : automaticShare;
            }

            return shares;
        }

        /// <summary>
        /// Adds whatever rounding lost or gained to the widest entry so the widths add up exactly.
        /// </summary>
        /// <param name="widths">Widths to correct in place.</param>
        /// <param name="targetTotal">The total the widths must add up to.</param>
        private static void AbsorbRoundingDrift(int[] widths, int targetTotal)
        {
            var drift = targetTotal - widths.Sum();
            if (drift == 0)
            {
                return;
            }

            var widestIndex = 0;
            for (var index = 1; index < widths.Length; index++)
            {
                if (widths[index] > widths[widestIndex])
                {
                    widestIndex = index;
                }
            }

            widths[widestIndex] += drift;
        }
    }
}
