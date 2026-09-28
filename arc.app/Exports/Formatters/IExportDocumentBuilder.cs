namespace arc.app.Exports.Formatters
{
    /// <summary>
    /// Builds a format-agnostic <see cref="ExportNode"/> document from an export context and its
    /// structural mapping tree. Shared by the JSON and XML writers.
    /// </summary>
    public interface IExportDocumentBuilder
    {
        /// <summary>
        /// Builds the intermediate document tree for the supplied context.
        /// </summary>
        /// <param name="context">The post-processed export context (rows, column keys and mapping).</param>
        /// <returns>The root <see cref="ExportNode"/> of the document.</returns>
        ExportNode Build(ExportFormatContext context);
    }
}
