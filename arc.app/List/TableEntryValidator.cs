using arc.app.Common;
using arc.common.Models;
using arc.common.Models.Lists;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.List
{
    /// <summary>
    /// Validates the Add and Edit Table Entry forms before the save event runs.
    /// Reads the crafted contents by key rather than by position, and treats a missing key as an empty value so
    /// that a change to the form payload produces a validation message instead of an unhandled exception.
    /// </summary>
    internal class TableEntryValidator : ISpecialValidator
    {
        private readonly string _message;

        /// <summary>
        /// Creates an instance of the validator for the supplied save payload.
        /// </summary>
        /// <param name="message">Serialized crafted pages payload submitted by the form.</param>
        public TableEntryValidator(string message)
        {
            _message = message;
        }

        /// <summary>
        /// Validates the submitted table entry.
        /// </summary>
        /// <returns>
        /// An empty string when the entry is valid, otherwise the language tag of the message to show:
        /// @GenA@ when the value is missing, @TabA@ when no table was selected,
        /// @TabPar@ when a self referencing table entry has no parent.
        /// </returns>
        public string ValidateMessage()
        {
            var tableEntry = JsonConvert.DeserializeObject<TableEntryCraftedModel>(_message);
            var contents = tableEntry?.Crafted?.FirstOrDefault()?.Contents;

            if (contents == null)
            {
                return "@GenA@";
            }

            if (string.IsNullOrWhiteSpace(ValueFor(contents, TableEntryFieldKeys.Value)))
            {
                return "@GenA@";
            }

            if (tableEntry.MetafListId == 0)
            {
                return "@TabA@";
            }

            var internalHierarchy = ValueFor(contents, TableEntryFieldKeys.InternalHierarchyParentOptionName);
            var parentId = ValueFor(contents, TableEntryFieldKeys.ParentId);
            var isFixed = IsFixed(ValueFor(contents, TableEntryFieldKeys.Fixed));

            if (!string.IsNullOrWhiteSpace(internalHierarchy) && !isFixed)
            {
                if (string.IsNullOrWhiteSpace(parentId) || parentId.Trim() == "0")
                {
                    return "@TabPar@";
                }
            }

            return "";
        }

        /// <summary>
        /// Reads a value out of the crafted contents by key, returning null when the key is absent.
        /// </summary>
        /// <param name="contents">The crafted contents submitted by the form.</param>
        /// <param name="key">The key to read, from <see cref="TableEntryFieldKeys"/>.</param>
        /// <returns>The value held against the key, or null.</returns>
        private static string ValueFor(List<KeyValueModel> contents, string key)
        {
            return contents.FirstOrDefault((r) => r.Key == key)?.Value;
        }

        /// <summary>
        /// Interprets the Fixed flag, which arrives as "Yes"/"No" from the form and "true"/"false" from the database.
        /// </summary>
        /// <param name="fixedValue">The raw flag value.</param>
        /// <returns>True when the entry is a system row.</returns>
        private static bool IsFixed(string fixedValue)
        {
            if (string.IsNullOrWhiteSpace(fixedValue))
            {
                return false;
            }

            var normalized = fixedValue.Trim();
            return normalized.Equals("Yes", System.StringComparison.OrdinalIgnoreCase)
                || normalized.Equals("true", System.StringComparison.OrdinalIgnoreCase);
        }
    }
}
