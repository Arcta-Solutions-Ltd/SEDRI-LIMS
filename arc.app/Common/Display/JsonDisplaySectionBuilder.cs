using arc.common.ExtensionMethods;
using arc.common.Models.Monitoring;
using arc.domain.Configuration.EventsConfig;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Common.Display
{
    /// <inheritdoc />
    public class JsonDisplaySectionBuilder : IJsonDisplaySectionBuilder
    {
        /// <summary>
        /// Text a JSON null is serialised to when it is collected as a scalar. Such a value is never worth showing.
        /// </summary>
        private const string NullLiteral = "null";

        private readonly ILogger<JsonDisplaySectionBuilder> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonDisplaySectionBuilder"/> class.
        /// </summary>
        /// <param name="logger">Logger used to report display rows that matched nothing in the payload.</param>
        public JsonDisplaySectionBuilder(ILogger<JsonDisplaySectionBuilder> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public List<JsonItemModel> Build(List<JsonItemModel> fields, EventConfig eventDetails, IReadOnlyList<string> preferredRootFieldOrder)
        {
            var fieldsToDisplay = new List<JsonItemModel>();

            if (eventDetails?.Display == null)
            {
                return fieldsToDisplay;
            }

            var sections = BuildSectionLookup(eventDetails);

            foreach (var labelToMatch in BuildRootMatchOrder(eventDetails, preferredRootFieldOrder))
            {
                TryAddRootField(fields, eventDetails, sections, fieldsToDisplay, labelToMatch);
            }

            LogUnmatchedDisplayRows(fields, eventDetails, fieldsToDisplay);

            return fieldsToDisplay;
        }

        /// <summary>
        /// Produces the sequence of field ids to look for at the payload root: the caller's preferred order
        /// first when supplied, then every configured display row so nothing is missed.
        /// </summary>
        /// <param name="eventDetails">Event configuration supplying the display rows.</param>
        /// <param name="preferredRootFieldOrder">Caller supplied ordering, may be null or empty.</param>
        /// <returns>Field ids in the order they should be matched. May contain duplicates; callers de-duplicate.</returns>
        private static IEnumerable<string> BuildRootMatchOrder(EventConfig eventDetails, IReadOnlyList<string> preferredRootFieldOrder)
        {
            if (preferredRootFieldOrder != null && preferredRootFieldOrder.Count > 0)
            {
                foreach (var fieldKey in preferredRootFieldOrder)
                {
                    yield return fieldKey;
                }
            }

            foreach (var entry in eventDetails.Display)
            {
                yield return entry.Label;
            }
        }

        /// <summary>
        /// Indexes the event's display sections by their normalized path so nested structures can be looked up
        /// while walking the payload.
        /// </summary>
        /// <param name="eventDetails">Event configuration supplying the sections.</param>
        /// <returns>Case-insensitive path to section lookup; empty when the event declares no sections.</returns>
        private static Dictionary<string, DisplaySectionConfig> BuildSectionLookup(EventConfig eventDetails)
        {
            var sections = new Dictionary<string, DisplaySectionConfig>(StringComparer.OrdinalIgnoreCase);

            if (eventDetails.DisplaySections == null)
            {
                return sections;
            }

            foreach (var section in eventDetails.DisplaySections.Where(s => !string.IsNullOrWhiteSpace(s?.Path)))
            {
                sections[section.Path.Trim()] = section;
            }

            return sections;
        }

        /// <summary>
        /// Matches one field id against the payload root and, when found, prepares and appends it.
        /// Fields already added are skipped so a preferred ordering cannot duplicate entries.
        /// </summary>
        /// <param name="fields">Parsed payload root fields.</param>
        /// <param name="eventDetails">Event configuration.</param>
        /// <param name="sections">Declared display sections by path.</param>
        /// <param name="fieldsToDisplay">Growing output list.</param>
        /// <param name="labelToMatch">Field id to resolve, matched case-insensitively.</param>
        private void TryAddRootField(
            List<JsonItemModel> fields,
            EventConfig eventDetails,
            Dictionary<string, DisplaySectionConfig> sections,
            List<JsonItemModel> fieldsToDisplay,
            string labelToMatch)
        {
            if (string.IsNullOrEmpty(labelToMatch))
            {
                return;
            }

            var entry = eventDetails.Display.FirstOrDefault(d => d.Label.IsSameAs(labelToMatch));
            if (entry == null)
            {
                return;
            }

            var field = fields?.FirstOrDefault(f => IdentifierOf(f).IsSameAs(entry.Label));
            if (field == null)
            {
                return;
            }

            if (fieldsToDisplay.Any(f => IdentifierOf(f).IsSameAs(IdentifierOf(field))))
            {
                return;
            }

            if (!IsNested(field) && IsEmptyCollection(field, entry, sections, IdentifierOf(field)))
            {
                return;
            }

            PrepareField(field, entry, eventDetails, sections, IdentifierOf(field));
            fieldsToDisplay.Add(field);
        }

        /// <summary>
        /// Applies the display rules for a single field in place: date trimming for scalars, grid preparation
        /// for collections and section filtering for nested objects.
        /// </summary>
        /// <param name="field">Field to prepare in place.</param>
        /// <param name="entry">Display row for the field; may be null for a field admitted only by a section.</param>
        /// <param name="eventDetails">Event configuration.</param>
        /// <param name="sections">Declared display sections by path.</param>
        /// <param name="path">Dot separated path of this field relative to the display root.</param>
        private void PrepareField(
            JsonItemModel field,
            DisplayConfig entry,
            EventConfig eventDetails,
            Dictionary<string, DisplaySectionConfig> sections,
            string path)
        {
            if (entry != null && entry.Date)
            {
                field.Contents = field.Contents != null && field.Contents.Length > 9
                    ? field.Contents[..10]
                    : "";
            }

            if (NullLiteral.IsSameAs(field.Contents))
            {
                field.Contents = "";
            }

            if (field.ArrayItems != null && field.ArrayItems.Count > 0)
            {
                PrepareCollection(field, entry, eventDetails, sections, path);
                return;
            }

            if (field.ChildItems != null && field.ChildItems.Count > 0 && sections.Count > 0)
            {
                // Events that declare no sections predate nested rendering; their nested objects are passed
                // through untouched so they keep rendering exactly as they do today.
                field.ChildItems = BuildChildItems(field.ChildItems, eventDetails, sections, path, out _);
            }
        }

        /// <summary>
        /// Rebuilds every row of a collection so each holds only the configured child fields, in display order,
        /// and attaches column headings when the field's display row is marked as a grid. Rows are padded to a
        /// common set of scalar columns so the grid stays aligned when individual rows omit a value.
        /// </summary>
        /// <param name="field">Collection field to prepare in place.</param>
        /// <param name="entry">Display row for the collection; may be null.</param>
        /// <param name="eventDetails">Event configuration.</param>
        /// <param name="sections">Declared display sections by path.</param>
        /// <param name="path">Dot separated path of the collection relative to the display root.</param>
        private void PrepareCollection(
            JsonItemModel field,
            DisplayConfig entry,
            EventConfig eventDetails,
            Dictionary<string, DisplaySectionConfig> sections,
            string path)
        {
            var isGrid = entry != null && entry.Grid;
            var rows = new List<JsonArrayModel>();
            var scalarColumns = new List<string>();

            foreach (var arrayItem in field.ArrayItems)
            {
                var childItems = BuildChildItems(arrayItem?.ChildItems, eventDetails, sections, path, out var rowScalarColumns);

                foreach (var column in rowScalarColumns)
                {
                    if (!scalarColumns.Any(c => c.IsSameAs(column)))
                    {
                        scalarColumns.Add(column);
                    }
                }

                rows.Add(new JsonArrayModel { ChildItems = childItems });
            }

            if (isGrid)
            {
                var columnOrder = OrderByDisplay(scalarColumns, eventDetails);
                foreach (var row in rows)
                {
                    row.ChildItems = PadRowToColumns(row.ChildItems, columnOrder, eventDetails);
                }

                field.ColumnHeadings = columnOrder
                    .Select(column => HeadingFor(column, eventDetails))
                    .ToList();

                _logger.LogDebug(
                    "Display grid '{Path}' for event {EventName}: {RowCount} rows, {ColumnCount} columns",
                    path,
                    eventDetails.EventName,
                    rows.Count,
                    columnOrder.Count);
            }

            field.ArrayItems = rows;
        }

        /// <summary>
        /// Selects the child fields of an object or collection row that should be shown, in display order.
        /// Scalars are date-trimmed; nested collections and objects are kept only where the event declares a
        /// display section for their path, and are then prepared recursively. Children admitted purely by a
        /// section take the section's translation as their label.
        /// </summary>
        /// <param name="childItems">Children as parsed from the payload; may be null.</param>
        /// <param name="eventDetails">Event configuration.</param>
        /// <param name="sections">Declared display sections by path.</param>
        /// <param name="parentPath">Dot separated path of the owning field relative to the display root.</param>
        /// <param name="scalarColumns">Field ids of the scalar children that were kept, in display order.</param>
        /// <returns>The children to show, scalars first and nested blocks last.</returns>
        private List<JsonItemModel> BuildChildItems(
            List<JsonItemModel> childItems,
            EventConfig eventDetails,
            Dictionary<string, DisplaySectionConfig> sections,
            string parentPath,
            out List<string> scalarColumns)
        {
            scalarColumns = new List<string>();
            var scalars = new List<JsonItemModel>();
            var nested = new List<JsonItemModel>();

            if (childItems == null || childItems.Count == 0)
            {
                return new List<JsonItemModel>();
            }

            var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var displayItem in eventDetails.Display)
            {
                var child = childItems.FirstOrDefault(c => IdentifierOf(c).IsSameAs(displayItem.Label));
                if (child == null || !handled.Add(IdentifierOf(child)))
                {
                    continue;
                }

                var childPath = CombinePath(parentPath, IdentifierOf(child));

                if (IsNested(child))
                {
                    if (TryAdmitNestedChild(child, displayItem, eventDetails, sections, parentPath))
                    {
                        nested.Add(child);
                    }

                    continue;
                }

                if (IsEmptyCollection(child, displayItem, sections, childPath))
                {
                    continue;
                }

                PrepareField(child, displayItem, eventDetails, sections, childPath);
                scalars.Add(child);
                scalarColumns.Add(IdentifierOf(child));
            }

            foreach (var child in childItems.Where(IsNested))
            {
                if (handled.Contains(IdentifierOf(child)))
                {
                    continue;
                }

                if (TryAdmitNestedChild(child, null, eventDetails, sections, parentPath))
                {
                    handled.Add(IdentifierOf(child));
                    nested.Add(child);
                }
            }

            return scalars.Concat(nested).ToList();
        }

        /// <summary>
        /// Decides whether a nested collection or object child is shown, and prepares it when it is.
        /// </summary>
        /// <param name="child">Nested child field.</param>
        /// <param name="displayItem">Display row for the child, or null when it has none.</param>
        /// <param name="eventDetails">Event configuration.</param>
        /// <param name="sections">Declared display sections by path.</param>
        /// <param name="parentPath">Dot separated path of the owning field.</param>
        /// <returns>True when the child should be shown.</returns>
        private bool TryAdmitNestedChild(
            JsonItemModel child,
            DisplayConfig displayItem,
            EventConfig eventDetails,
            Dictionary<string, DisplaySectionConfig> sections,
            string parentPath)
        {
            var childPath = CombinePath(parentPath, IdentifierOf(child));

            if (sections.Count == 0)
            {
                // No sections declared, so this event predates nested rendering. Pass the child through
                // untouched to preserve how it renders today.
                return displayItem != null;
            }

            if (!sections.TryGetValue(childPath, out var section))
            {
                _logger.LogDebug(
                    "Display section '{Path}' is not declared for event {EventName}, so the nested structure is not shown",
                    childPath,
                    eventDetails.EventName);
                return false;
            }

            if (displayItem == null && !string.IsNullOrWhiteSpace(section.Translation))
            {
                child.Label = section.Translation;
            }

            PrepareField(child, displayItem, eventDetails, sections, childPath);
            return true;
        }

        /// <summary>
        /// Adds an empty placeholder for any grid column a row does not carry, so every row presents the same
        /// columns in the same order.
        /// </summary>
        /// <param name="childItems">The row's children.</param>
        /// <param name="columnOrder">Canonical scalar column field ids.</param>
        /// <param name="eventDetails">Event configuration, used to label placeholders.</param>
        /// <returns>The row's children padded and ordered to match <paramref name="columnOrder"/>, nested blocks last.</returns>
        private static List<JsonItemModel> PadRowToColumns(
            List<JsonItemModel> childItems,
            IReadOnlyList<string> columnOrder,
            EventConfig eventDetails)
        {
            var padded = new List<JsonItemModel>();

            foreach (var column in columnOrder)
            {
                var existing = childItems.FirstOrDefault(c => IdentifierOf(c).IsSameAs(column));
                padded.Add(existing ?? new JsonItemModel
                {
                    Key = column,
                    Label = HeadingFor(column, eventDetails),
                    Contents = ""
                });
            }

            padded.AddRange(childItems.Where(IsNested));

            return padded;
        }

        /// <summary>
        /// Sorts field ids into the order their display rows are configured in.
        /// </summary>
        /// <param name="fieldIds">Field ids to sort.</param>
        /// <param name="eventDetails">Event configuration supplying the display order.</param>
        /// <returns>The field ids in display order; ids with no display row keep their relative order at the end.</returns>
        private static List<string> OrderByDisplay(IEnumerable<string> fieldIds, EventConfig eventDetails)
        {
            return fieldIds
                .OrderBy(fieldId =>
                {
                    var index = eventDetails.Display.FindIndex(d => d.Label.IsSameAs(fieldId));
                    return index < 0 ? int.MaxValue : index;
                })
                .ToList();
        }

        /// <summary>
        /// Returns the language tag configured for a field id, falling back to the field id itself.
        /// </summary>
        /// <param name="fieldId">Field id to look up.</param>
        /// <param name="eventDetails">Event configuration supplying the display rows.</param>
        /// <returns>The configured translation tag, or the field id when none is configured.</returns>
        private static string HeadingFor(string fieldId, EventConfig eventDetails)
        {
            var entry = eventDetails.Display.FirstOrDefault(d => d.Label.IsSameAs(fieldId));
            return string.IsNullOrWhiteSpace(entry?.Translation) ? fieldId : entry.Translation;
        }

        /// <summary>
        /// Returns the untranslated field id for an item, preferring <see cref="JsonItemModel.Key"/> and falling
        /// back to <see cref="JsonItemModel.Label"/> for items built before the key was populated.
        /// </summary>
        /// <param name="item">Item to identify.</param>
        /// <returns>The field id, or an empty string when the item is null.</returns>
        private static string IdentifierOf(JsonItemModel item)
            => string.IsNullOrEmpty(item?.Key) ? item?.Label ?? "" : item.Key;

        /// <summary>
        /// Detects a collection or nested object that arrived as an empty or null scalar. Such a field carries no
        /// rows to show, so keeping it would render a stray "null" cell where a grid or block was expected.
        /// </summary>
        /// <param name="child">Scalar child field under consideration.</param>
        /// <param name="entry">Display row for the child; may be null.</param>
        /// <param name="sections">Declared display sections by path.</param>
        /// <param name="childPath">Dot separated path of the child relative to the display root.</param>
        /// <returns>True when the field should be dropped.</returns>
        private static bool IsEmptyCollection(
            JsonItemModel child,
            DisplayConfig entry,
            Dictionary<string, DisplaySectionConfig> sections,
            string childPath)
        {
            var expectedToNest = (entry != null && entry.Grid) || sections.ContainsKey(childPath ?? "");
            return expectedToNest
                && (string.IsNullOrWhiteSpace(child.Contents) || NullLiteral.IsSameAs(child.Contents));
        }

        /// <summary>
        /// Returns whether an item holds a nested collection or object rather than a scalar value.
        /// </summary>
        /// <param name="item">Item to test.</param>
        /// <returns>True when the item has array or child items.</returns>
        private static bool IsNested(JsonItemModel item)
            => (item?.ArrayItems != null && item.ArrayItems.Count > 0)
               || (item?.ChildItems != null && item.ChildItems.Count > 0);

        /// <summary>
        /// Joins a parent path and a field id into a display section path.
        /// </summary>
        /// <param name="parentPath">Parent path; may be empty for the display root.</param>
        /// <param name="fieldId">Field id to append.</param>
        /// <returns>The combined dot separated path.</returns>
        private static string CombinePath(string parentPath, string fieldId)
            => string.IsNullOrEmpty(parentPath) ? fieldId : $"{parentPath}.{fieldId}";

        /// <summary>
        /// Reports the shape of the match so a site with no debugger can tell a configuration problem from a
        /// data problem, and warns when nothing at all matched.
        /// </summary>
        /// <param name="fields">Parsed payload root fields.</param>
        /// <param name="eventDetails">Event configuration.</param>
        /// <param name="fieldsToDisplay">Fields that were matched.</param>
        private void LogUnmatchedDisplayRows(List<JsonItemModel> fields, EventConfig eventDetails, List<JsonItemModel> fieldsToDisplay)
        {
            if (eventDetails.Display.Count == 0)
            {
                return;
            }

            var matched = new HashSet<string>(fieldsToDisplay.Select(IdentifierOf), StringComparer.OrdinalIgnoreCase);
            var unmatched = eventDetails.Display
                .Select(d => d.Label)
                .Where(label => !string.IsNullOrEmpty(label) && !matched.Contains(label))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (fieldsToDisplay.Count == 0)
            {
                _logger.LogWarning(
                    "Display: none of the {DisplayCount} display rows for event {EventName} matched the {RootFieldCount} payload root fields ({RootFields}); check DisplayRoot and the display labels",
                    eventDetails.Display.Count,
                    eventDetails.EventName,
                    fields?.Count ?? 0,
                    string.Join(", ", (fields ?? new List<JsonItemModel>()).Select(IdentifierOf)));
                return;
            }

            _logger.LogDebug(
                "Display: event {EventName} matched {MatchedCount} of {DisplayCount} display rows at the payload root; unmatched root labels: {UnmatchedLabels}",
                eventDetails.EventName,
                fieldsToDisplay.Count,
                eventDetails.Display.Count,
                unmatched.Count == 0 ? "(none)" : string.Join(", ", unmatched));
        }
    }
}
