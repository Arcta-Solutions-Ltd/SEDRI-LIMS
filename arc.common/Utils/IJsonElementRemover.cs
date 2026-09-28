namespace arc.common.Utils
{
    /// <summary>
    /// Defines a contract for removing elements from JSON data based on their values.
    /// </summary>
    public interface IJsonElementRemover
    {
        /// <summary>
        /// Removes elements from a JSON string where their values match the specified value.
        /// </summary>
        /// <param name="json">The JSON string to process.</param>
        /// <param name="value">The value used to identify elements to remove.</param>
        /// <returns>
        /// A JSON string with the specified elements removed.
        /// </returns>
        string RemoveElementsByValue(string json, string value);
    }
}
