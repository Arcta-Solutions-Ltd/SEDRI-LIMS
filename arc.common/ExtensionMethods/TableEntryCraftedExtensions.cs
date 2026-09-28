using arc.common.Models;
using arc.common.Models.Lists;
using System.Collections.Generic;
using System.Globalization;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Builds the crafted page contents for the Add and Edit Table Entry forms.
    /// The UI reads a crafted page as a list of Key/value pairs; contents that serialise to a bare object are
    /// discarded by the form shell, which is why these two forms must emit the list shape.
    /// </summary>
    public static class TableEntryCraftedExtensions
    {
        /// <summary>
        /// Builds the crafted contents for the Add Table Entry form from the list being added to.
        /// A new entry has no value, no parent and is never a system row, so those keys are emitted empty.
        /// </summary>
        /// <param name="list">The list (table) the new entry will belong to.</param>
        /// <returns>Key/value pairs for the tableentrypage crafted page.</returns>
        public static List<JsonKeyValuePairModel> ToCraftedContents(this ListByIdQueryModel list)
        {
            return BuildContents(
                id: string.Empty,
                value: string.Empty,
                enabled: string.IsNullOrWhiteSpace(list?.Enabled) ? "Yes" : list.Enabled,
                parentId: string.Empty,
                isFixed: "No",
                optionName: list?.OptionName,
                internalHierarchyParentOptionName: list?.InternalHierarchyParentOptionName,
                listId: list?.Id ?? 0);
        }

        /// <summary>
        /// Builds the crafted contents for the Edit Table Entry form from the entry and its list.
        /// Carries the entry's current value, enabled flag, fixed flag and parent ids so the form opens populated.
        /// </summary>
        /// <param name="entry">The list item being edited, including its comma separated parent ids.</param>
        /// <returns>Key/value pairs for the edittableentrypage crafted page.</returns>
        public static List<JsonKeyValuePairModel> ToCraftedContents(this ListItemWithOptionModel entry)
        {
            return BuildContents(
                id: entry?.Id.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                value: entry?.Value ?? string.Empty,
                enabled: string.IsNullOrWhiteSpace(entry?.Enabled) ? "Yes" : entry.Enabled,
                parentId: entry?.ParentId ?? string.Empty,
                isFixed: entry?.Fixed ?? "No",
                optionName: entry?.OptionName,
                internalHierarchyParentOptionName: entry?.InternalHierarchyParentOptionName,
                listId: entry?.ListId ?? 0);
        }

        /// <summary>
        /// Emits the full key set in a fixed order so the Add and Edit forms behave identically.
        /// Every key is always present, because the UI treats a missing key and an empty key differently:
        /// a missing parent option name silently removes the Parent field from the form.
        /// </summary>
        private static List<JsonKeyValuePairModel> BuildContents(string id, string value, string enabled,
            string parentId, string isFixed, string optionName, string internalHierarchyParentOptionName, int listId)
        {
            return new List<JsonKeyValuePairModel>
            {
                new() { Key = TableEntryFieldKeys.Id, value = id },
                new() { Key = TableEntryFieldKeys.Value, value = value },
                new() { Key = TableEntryFieldKeys.Enabled, value = enabled },
                new() { Key = TableEntryFieldKeys.ParentId, value = parentId },
                new() { Key = TableEntryFieldKeys.Fixed, value = isFixed },
                new() { Key = TableEntryFieldKeys.OptionName, value = optionName },
                new() { Key = TableEntryFieldKeys.InternalHierarchyParentOptionName, value = internalHierarchyParentOptionName },
                new() { Key = TableEntryFieldKeys.ListId, value = listId.ToString(CultureInfo.InvariantCulture) }
            };
        }
    }
}
