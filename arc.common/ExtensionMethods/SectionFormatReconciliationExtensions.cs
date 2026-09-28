using System;
using System.Collections.Generic;
using System.Linq;
using arc.common.Models.Reports.ReportDesigner;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Reconciles section field and grid header bindings when a format's column geometry shrinks.
    /// </summary>
    public static class SectionFormatReconciliationExtensions
    {
        /// <summary>
        /// Removes section fields whose Column exceeds the format column count supplied in the same save payload.
        /// A capacity of zero clears all scalar field placements.
        /// </summary>
        /// <param name="section">The section being written.</param>
        /// <param name="formatColumnCapacity">Field column counts for formats changed in this save.</param>
        /// <returns>The number of fields removed.</returns>
        public static int TrimSectionFieldsToFormatCapacity(
            ReportSectionModel section,
            IReadOnlyDictionary<string, int> formatColumnCapacity)
        {
            if (section?.Fields == null || section.Fields.Count == 0)
            {
                return 0;
            }

            var formatName = section.Format.NormalisedConfigName();
            if (formatName.Length == 0 || !formatColumnCapacity.TryGetValue(formatName, out var capacity))
            {
                return 0;
            }

            var fieldCount = section.Fields.Count;

            if (capacity <= 0)
            {
                section.Fields = new List<ReportSectionFieldModel>();
                return fieldCount;
            }

            var trimmedFields = section.Fields
                .Where(field => field != null && field.Column >= 1 && field.Column <= capacity)
                .ToList();

            var removedCount = fieldCount - trimmedFields.Count;
            if (removedCount > 0)
            {
                section.Fields = trimmedFields;
            }

            return removedCount;
        }

        /// <summary>
        /// Trims section grid Head arrays when they exceed the width-slot count defined on the format.
        /// </summary>
        /// <param name="section">The section being written.</param>
        /// <param name="formatsByName">Formats changed in this save, keyed by normalised name.</param>
        /// <returns>The number of grid header slots removed across all bindings.</returns>
        public static int TrimSectionGridHeadsToFormatCapacity(
            ReportSectionModel section,
            IReadOnlyDictionary<string, CustomFormatModel> formatsByName)
        {
            if (section?.Grids == null || section.Grids.Count == 0)
            {
                return 0;
            }

            var formatName = section.Format.NormalisedConfigName();
            if (formatName.Length == 0 || !formatsByName.TryGetValue(formatName, out var format))
            {
                return 0;
            }

            var formatGrids = format.Grids ?? new List<ReportGridModel>();
            if (formatGrids.Count == 0)
            {
                return 0;
            }

            var removedHeaderCount = 0;
            var capacity = formatGrids.Count;

            for (var index = 0; index < section.Grids.Count && index < capacity; index++)
            {
                var sectionGrid = section.Grids[index];
                if (sectionGrid?.Head == null || sectionGrid.Head.Count == 0)
                {
                    continue;
                }

                var formatGrid = index < formatGrids.Count
                    ? formatGrids[index]
                    : formatGrids[formatGrids.Count - 1];
                var columnCount = GetFormatGridColumnCount(formatGrid);

                if (sectionGrid.Head.Count <= columnCount)
                {
                    continue;
                }

                removedHeaderCount += sectionGrid.Head.Count - columnCount;
                sectionGrid.Head = sectionGrid.Head.Take(columnCount).ToList();
            }

            return removedHeaderCount;
        }

        /// <summary>
        /// Builds a map of format name to field column count from the formats included in the same save.
        /// </summary>
        /// <param name="changedFormats">The custom formats the designer changed in this save.</param>
        /// <returns>Normalised format name to the number of field columns the format defines.</returns>
        public static Dictionary<string, int> BuildFormatColumnCapacityMap(IEnumerable<CustomFormatModel> changedFormats)
        {
            var capacity = new Dictionary<string, int>(StringComparer.Ordinal);

            foreach (var format in changedFormats ?? [])
            {
                var formatName = format?.Name.NormalisedConfigName();
                if (string.IsNullOrEmpty(formatName))
                {
                    continue;
                }

                capacity[formatName] = format.Columns?.Count ?? 0;
            }

            return capacity;
        }

        /// <summary>
        /// Builds a map of format name to format model from the formats included in the same save.
        /// </summary>
        /// <param name="changedFormats">The custom formats the designer changed in this save.</param>
        /// <returns>Normalised format name to the format model.</returns>
        public static Dictionary<string, CustomFormatModel> BuildFormatMap(IEnumerable<CustomFormatModel> changedFormats)
        {
            var formats = new Dictionary<string, CustomFormatModel>(StringComparer.Ordinal);

            foreach (var format in changedFormats ?? [])
            {
                var formatName = format?.Name.NormalisedConfigName();
                if (string.IsNullOrEmpty(formatName))
                {
                    continue;
                }

                formats[formatName] = format;
            }

            return formats;
        }

        /// <summary>
        /// Returns the number of width slots a format grid position defines.
        /// </summary>
        /// <param name="formatGrid">The format grid geometry.</param>
        /// <returns>The number of pipe-separated width entries.</returns>
        private static int GetFormatGridColumnCount(ReportGridModel formatGrid)
        {
            if (string.IsNullOrWhiteSpace(formatGrid?.Width))
            {
                return 0;
            }

            return formatGrid.Width
                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Length;
        }
    }
}
