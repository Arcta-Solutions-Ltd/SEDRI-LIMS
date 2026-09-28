using System.Threading.Tasks;

namespace arc.app.Instruments.CustomImport
{
    /// <summary>
    /// Resolves raw imported values to the stored representation used for matching and saving. For list/tag fields this
    /// converts a display value (which may be translated) to its list item id, so that record matching and interface
    /// criteria evaluation are language independent.
    /// </summary>
    public interface IImportValueResolver
    {
        /// <summary>
        /// Resolves a raw value for a field in the given bucket. List fields return the list item id (as a string);
        /// numeric/date/text fields return the trimmed raw value. Values that are already numeric ids pass through.
        /// </summary>
        /// <param name="bucket">Normalized bucket (patient / specimen / culture / ast).</param>
        /// <param name="fieldName">Source field name.</param>
        /// <param name="rawValue">Raw value from the inbound file.</param>
        /// <returns>The resolved value; the original value when no resolution applies or a list value cannot be matched.</returns>
        Task<string> ResolveAsync(string bucket, string fieldName, string rawValue);
    }
}
