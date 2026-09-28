using arc.common.Models.Lists;

namespace arc.common.ExtensionMethods
{
    /// <summary>
    /// Extension methods for list query models used in configuration.
    /// </summary>
    public static class ListModelExtensions
    {
        /// <summary>
        /// Returns true when the list references a parent list via <see cref="ListByIdQueryModel.ParentListId"/>.
        /// </summary>
        /// <param name="list">The list model to inspect.</param>
        /// <returns>True when this list is a child table.</returns>
        public static bool IsChildTable(this ListByIdQueryModel list)
        {
            return list?.ParentListId is > 0;
        }
    }
}
