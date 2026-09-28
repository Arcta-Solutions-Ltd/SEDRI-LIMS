using arc.common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;

namespace arc.app.Configuration
{
    /// <summary>
    /// Maps configurable field type names to list item ids in FieldTypeList (list 109).
    /// </summary>
    public class FieldTypeList : IFieldTypeList
    {
        private readonly Dictionary<int, string> _typeList = new Dictionary<int, string>
        {
            { 450, "singleline"},
            { 451, "multiline" },
            { 452, "text"},
            { 453, "combobox"},
            { 454, "dropdown"},
            { 460, "hierarchicalpicker"},
            { 455, "date"},
            { 456, "time"},
            { 457, "toggle"},
            { 458, "number"},
            { 459, "fieldgrid"},
            { 149, "upload"},
            { 150, "age"},
            { 151, "radio"}
        };

        private readonly Dictionary<int, string> _gridTypeList = new Dictionary<int, string>
        {
            { 462, "small"},
            { 463, "medium" },
            { 464, "narrow"},
            { 465, "smaller"},
            { 466, "wide"},
            { 467, "standard"}
        };

        /// <summary>
        /// Returns the FieldTypeList item id for a field type name.
        /// </summary>
        /// <param name="name">The field type name, for example "radio" or "dropdown".</param>
        /// <returns>The matching list item id.</returns>
        /// <exception cref="InvalidOperationException">When the type name is not recognised.</exception>
        public int GetIdFromName(string name)
        {
            if (!TryGetIdFromName(name, out var id))
            {
                throw new InvalidOperationException($"Unknown field type '{name ?? ""}'.");
            }

            return id;
        }

        /// <summary>
        /// Attempts to resolve a field type name to its FieldTypeList item id.
        /// </summary>
        /// <param name="name">The field type name. May be null or empty.</param>
        /// <param name="id">The resolved id when the lookup succeeds.</param>
        /// <returns>True when the type name is recognised.</returns>
        public bool TryGetIdFromName(string name, out int id)
        {
            id = 0;
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            var match = _typeList.FirstOrDefault(e => e.Value.IsSameAs(name));
            if (string.IsNullOrEmpty(match.Value))
            {
                return false;
            }

            id = match.Key;
            return true;
        }

        /// <summary>
        /// Returns the field type name for a FieldTypeList item id.
        /// </summary>
        /// <param name="id">The list item id.</param>
        /// <returns>The field type name.</returns>
        public string GetNameFromId(int id)
        {
            return _typeList[id];
        }

        /// <summary>
        /// Returns the grid column width list item id for a width name.
        /// </summary>
        /// <param name="name">The width name. Defaults to "medium" when blank.</param>
        /// <returns>The matching grid width list item id.</returns>
        public int GetGridIdFromName(string name)
        {
            name = string.IsNullOrWhiteSpace(name) ? "medium" : name;
            return _gridTypeList.First(e => e.Value.ToLower() == name.ToLower()).Key;
        }

        /// <summary>
        /// Returns the grid column width name for a list item id.
        /// </summary>
        /// <param name="id">The grid width list item id.</param>
        /// <returns>The width name.</returns>
        public string GetGridNameFromId(int id)
        {
            return _gridTypeList[id];
        }
    }
}
